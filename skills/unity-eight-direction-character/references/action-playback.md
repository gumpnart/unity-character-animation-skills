# Action setup and playback

The enhanced EightDirectionCharacter is the single owner of layer 0. Its action
implementation lives in EightDirectionCharacter.Actions.cs as a partial class.
Copy both files and CharacterActionDefinition.cs into Runtime; do not attach a
second action component. The character field list includes testAction afterward.
Copy CharacterActionAnimationBuilder.cs into Editor.

## Playback contract

| Mode | Behavior |
|---|---|
| One-shot + ReturnToLocomotion | Finish, restore profile depth/appearance, then latest requested idle/walk |
| One-shot + HoldLastPose | Fire completion once and retain terminal pose until TryEndAction |
| Loop | Keep looping until TryEndAction or an allowed cancellation |
| Cancel | Allowed only when interruptible; restore validated current idle/walk |

Facing is locked for the active action. The driver still stores new movement
requests but does not change actions into walk states. Latest input chooses the
facing/idle/walk used when released. World movement is owned by the caller; a
locked animation does not stop the physics body automatically. Check
IsPerformingAction in movement code when a stationary action must stop translation.

Requests while busy are rejected rather than queued or implicitly replacing an
action. If replacement is desired, cancel explicitly and check its result, then
start the new action. Noninterruptible actions still finish normally; a completed
held pose or intentional loop can be released with TryEndAction.

Return validates the entire target appearance and state before changing it. If
the desired return direction lacks content, the action remains at its terminal
pose (a loop continues) and reports an error. Correct the content or request a
valid facing/zero movement, then retry release. No partial visual turn occurs.

Equipment swaps are blocked by default during an action; allowEquipmentSwap can
enable swaps without restarting its clip. The replacement grip still must be
compatible with the action. Items don't own action animations or a separate rig.

## Calling code

```csharp
using RpgEightDirection;

// Action assets and character references supplied by the project / Inspector.
if (!character.TryPlayAction(miningAction, out string error))
    Debug.LogError(error);

// An aimed action can select a supported facing at entry.
character.TryPlayAction(castAction, out error, Direction8.NorthEast);

// Release a loop/held pose or an ended one-shot; loop release is immediate.
character.TryEndAction(out error);

// Interrupt an action only if its asset permits it.
character.TryCancelAction(out error);
```

These calls illustrate separate moments, not a script that plays every action
in sequence. A new action requires the previous one to be released first.

## Cue events

The generated clips contain AnimationEvents targeting OnActionMarker on the same
root component. Only events from the currently active action state are forwarded.
Markers must be strictly inside the clip (not exactly 0 or duration); use
ActionStarted/ActionCompleted for boundaries to avoid duplicate loop-end cues.

```csharp
private void OnEnable()
{
    character.ActionStarted += OnStarted;
    character.ActionMarker += OnMarker;
    character.ActionCompleted += OnCompleted;
    character.ActionCancelled += OnCancelled;
}

private void OnDisable()
{
    character.ActionStarted -= OnStarted;
    character.ActionMarker -= OnMarker;
    character.ActionCompleted -= OnCompleted;
    character.ActionCancelled -= OnCancelled;
}

private void OnStarted(string actionId) { /* optional presentation */ }
private void OnMarker(string actionId, string markerName)
{
    // Route to the game's audio, VFX, or interaction logic as authorized.
    // Example: actionId == "MineOre" and markerName == "Impact".
}
private void OnCompleted(string actionId) { /* optional presentation */ }
private void OnCancelled(string actionId) { /* optional presentation */ }
```

Gameplay cues are notifications. They do not automatically apply damage, spend
items, or trigger external effects. Animation callbacks can arrive during Animator
evaluation; queue requests that change Animator state to the next gameplay Update.
Unsubscribe listeners on disable. Do not put a second clip-timer loop beside the
Animator event system or assume events are replayed by editor scrubbing.

## Test matrix

Play from idle and walking in each requested direction; try held/loop behavior,
equipment changes, cancellation, and return input that changed during the action.
Verify markers at normal and slow speeds and over several loop cycles. Pause
Animator speed at zero: a visual action should not finish from a wall-clock timer.
Check no duplicate completion for a held pose, no cues after cancel, and no
automatic locomotion state change while the action is still active.

The template uses normal Animator evaluation, nonblended layer-0 states at speed
1, and the definition's durations. Keep generated data/clips consistent. Other
layer-0 writers, manual Animator.Update calls, automatic transitions, changing
state speed per direction, or AnimatorOverrideController require integration
work and additional verification. New actions can use the same standard controller;
do not rerun the base locomotion builder and assign a fresh controller without
deliberately restoring its action states.
