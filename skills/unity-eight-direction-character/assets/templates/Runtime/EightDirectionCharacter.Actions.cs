using System;
using UnityEngine;

namespace RpgEightDirection
{
    // Part of EightDirectionCharacter, not a second Animator owner/component.
    public sealed partial class EightDirectionCharacter
    {
        public CharacterActionDefinition testAction;
        private CharacterActionDefinition activeAction;
        private ActionDirectionTrack activeTrack;
        private int activeActionState;
        private bool actionCompleted;

        public bool IsPerformingAction { get { return activeAction != null; } }
        public bool IsHoldingCompletedAction { get { return activeAction != null && actionCompleted; } }
        public string ActiveActionId { get { return activeAction != null ? activeAction.actionId : null; } }
        public event Action<string> ActionStarted;
        public event Action<string, string> ActionMarker;
        public event Action<string> ActionCompleted;
        public event Action<string> ActionCancelled;

        // Aiming can supply a facing; otherwise use current physical facing.
        public bool TryPlayAction(CharacterActionDefinition action, out string error, Direction8? facing = null)
        {
            error = null;
            if (activePose == null || !enabled || animator == null || action == null)
                return Fail("Initialize an enabled character and supply an action asset.", out error);
            if (activeAction != null)
                return Fail("An action is active. End or cancel it before starting another.", out error);
            Direction8 direction = facing ?? Facing;
            if (!TryValidateAction(action, direction, out error)) return false;
            int state = Animator.StringToHash("Base Layer." + action.StateName(direction));
            if (!animator.HasState(0, state))
                return Fail("Missing action Animator state: " + action.StateName(direction), out error);
            AppearancePlan plan;
            if (!TryPrepare(direction, equipped, out plan, out error)) return false;
            ActionDirectionTrack track;
            action.TryGet(direction, out track); // Validated above.
            Commit(plan);
            activePose = plan.pose;
            activeAction = action; activeTrack = track; activeActionState = state;
            actionCompleted = false; isWalking = false; gaitPhase = 0f;
            lastError = null;
            animator.Play(state, 0, 0f);
            if (ActionStarted != null) ActionStarted(action.actionId);
            return true;
        }

        // Validates geometry/sorting/appearance without requiring generated states.
        public bool TryValidateAction(CharacterActionDefinition action, Direction8 direction, out string error)
        {
            error = null;
            if (action == null) return Fail("Action asset is missing.", out error);
            if (!action.TryValidate(out error)) return false;
            ActionDirectionTrack track;
            if (!action.TryGet(direction, out track))
                return Fail("Action has no authored track for " + direction, out error);
            if (!TryValidateDirection(direction, out error)) return false;
            foreach (var pose in track.poses)
                foreach (var order in pose.rendererOrders)
                {
                    bool registered = false;
                    foreach (var binding in bindings)
                        if (binding.category == order.category) registered = true;
                    if (!registered) return Fail("Unbound action renderer override: " + order.category, out error);
                }
            return true;
        }

        private void UpdateAction()
        {
            if (actionCompleted) return;
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.fullPathHash != activeActionState || activeAction.loop || state.normalizedTime < 1f) return;
            if (activeAction.completion == ActionCompletion.HoldLastPose)
            {
                actionCompleted = true;
                if (ActionCompleted != null) ActionCompleted(activeAction.actionId);
                return;
            }
            string error;
            if (!TryEndAction(out error) && error != lastError)
            {
                Debug.LogError(error, this);
                lastError = error;
            }
        }

        // Explicitly finish a loop, release a held pose, or release an ended one-shot.
        // Loop release is immediate; a caller can request it at a chosen cycle boundary.
        public bool TryEndAction(out string error)
        {
            error = null;
            if (activeAction == null) return Fail("There is no action to finish.", out error);
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (!activeAction.loop && !actionCompleted &&
                (state.fullPathHash != activeActionState || state.normalizedTime < 1f))
                return Fail("One-shot action is still running. Use cancel if interruptible.", out error);
            return TryReleaseAction(false, out error);
        }

        public bool TryCancelAction(out string error)
        {
            if (activeAction == null) return Fail("There is no action to cancel.", out error);
            if (!activeAction.interruptible) return Fail("This action does not allow cancellation.", out error);
            return TryReleaseAction(true, out error);
        }

        private bool TryReleaseAction(bool cancelled, out string error)
        {
            Vector2 velocity = useInspectorTest ? testScreenVelocity : requestedVelocity;
            if (!Direction8Utility.IsFinite(velocity)) velocity = Vector2.zero;
            bool walking = velocity.magnitude > Mathf.Max(0f, deadZone);
            Direction8 direction = Direction8Utility.Select(velocity, Facing, deadZone, hysteresisDegrees);
            // A missing target state/variant keeps the entire action pose intact.
            if (!TryChangeState(direction, walking, out error)) return false;
            string id = activeAction.actionId;
            bool notifyCompletion = !actionCompleted;
            activeAction = null; activeTrack = null; actionCompleted = false;
            activeActionState = 0; lastError = null;
            if (cancelled) { if (ActionCancelled != null) ActionCancelled(id); }
            else if (notifyCompletion && ActionCompleted != null) ActionCompleted(id);
            return true;
        }

        // Called by generated AnimationEvents on this same root component.
        public void OnActionMarker(AnimationEvent marker)
        {
            if (activeAction == null || actionCompleted ||
                marker.animatorStateInfo.fullPathHash != activeActionState) return;
            if (ActionMarker != null) ActionMarker(activeAction.actionId, marker.stringParameter);
        }

        private bool TryApplyActionDepth()
        {
            if (activeAction == null) return false;
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            float normalized = state.fullPathHash == activeActionState ? state.normalizedTime : 0f;
            float phase = activeAction.loop ? Mathf.Repeat(normalized, 1f) : Mathf.Clamp01(normalized);
            float time = phase * activeTrack.duration;
            ActionPoseKey key = activeTrack.poses[0];
            foreach (var candidate in activeTrack.poses)
            {
                if (candidate.time > time) break;
                key = candidate;
            }
            LimbOrders depth = key.overrideLimbDepth ? key.limbDepth : activePose.idleDepth;
            legL.sortingOrder = depth.legL; legR.sortingOrder = depth.legR;
            armL.sortingOrder = depth.armL; armR.sortingOrder = depth.armR;
            // Reset a previous key's renderer overrides before applying this key.
            foreach (var binding in bindings)
            {
                int order = 0;
                foreach (var baseline in activePose.rendererOrders)
                    if (baseline.category == binding.category) order = baseline.order;
                foreach (var replacement in key.rendererOrders)
                    if (replacement.category == binding.category) order = replacement.order;
                binding.resolver.GetComponent<SpriteRenderer>().sortingOrder = order;
            }
            return true;
        }

        [ContextMenu("Play Test Action")]
        private void PlayTestAction()
        {
            if (!Application.isPlaying) return;
            string error;
            if (!TryPlayAction(testAction, out error)) Debug.LogError(error, this);
        }

        [ContextMenu("Cancel Test Action")]
        private void CancelTestAction()
        {
            if (!Application.isPlaying) return;
            string error;
            if (!TryCancelAction(out error)) Debug.LogError(error, this);
        }

        [ContextMenu("End Test Action")]
        private void EndTestAction()
        {
            if (!Application.isPlaying) return;
            string error;
            if (!TryEndAction(out error)) Debug.LogError(error, this);
        }
    }
}
