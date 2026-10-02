using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RpgEightDirection
{
    public enum ActionCompletion { ReturnToLocomotion, HoldLastPose }
    public enum ActionCurveStyle { Linear, SmoothClamped }

    [Serializable]
    public struct ActionBoneOffset
    {
        public string bonePath;
        public Vector2 positionPixels;
        public float rotationZ;
    }

    [Serializable]
    public struct ActionMarkerKey
    {
        public float time;
        public string name;
    }

    [Serializable]
    public sealed class ActionPoseKey
    {
        public float time;
        // Omitted bones return to the direction's captured rest pose at this key.
        public ActionBoneOffset[] bones = new ActionBoneOffset[0];
        public bool overrideLimbDepth;
        public LimbOrders limbDepth;
        // Per-renderer overrides last until the next key, then reset to profile orders.
        public RendererOrder[] rendererOrders = new RendererOrder[0];
    }

    [Serializable]
    public sealed class ActionDirectionTrack
    {
        public Direction8 direction;
        [Min(0.001f)] public float duration = 1f;
        public ActionCurveStyle curves = ActionCurveStyle.Linear;
        public ActionPoseKey[] poses =
        {
            new ActionPoseKey { time = 0f }, new ActionPoseKey { time = 1f }
        };
        public ActionMarkerKey[] markers = new ActionMarkerKey[0];
    }

    [CreateAssetMenu(menuName = "Eight Direction Character/Action")]
    public sealed class CharacterActionDefinition : ScriptableObject
    {
        // Open-ended string ID; no attack/cast/etc. enum or baked-in catalog.
        public string actionId = "CustomAction";
        [TextArea] public string intent;
        public bool loop;
        public bool interruptible = true;
        public bool allowEquipmentSwap;
        public ActionCompletion completion = ActionCompletion.ReturnToLocomotion;
        public ActionDirectionTrack[] directions = CreateDirections();

        public string StateName(Direction8 direction)
        {
            return "Action_" + actionId + "_" + direction;
        }

        public bool TryGet(Direction8 direction, out ActionDirectionTrack track)
        {
            track = null;
            if (directions == null) return false;
            foreach (var entry in directions)
            {
                if (entry == null) return false;
                if (entry.direction != direction) continue;
                if (track != null) return false;
                track = entry;
            }
            return track != null;
        }

        public bool TryValidate(out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(actionId) || actionId.Length > 64 ||
                !Regex.IsMatch(actionId, "^[A-Za-z][A-Za-z0-9_]*$"))
                return Fail("Action ID must start with a letter and use letters, digits, or underscores (max 64).", out error);
            if (!Enum.IsDefined(typeof(ActionCompletion), completion) ||
                directions == null || directions.Length == 0 || directions.Length > 8)
                return Fail("Configure completion and one to eight direction tracks.", out error);
            var seenDirections = new HashSet<Direction8>();
            foreach (var track in directions)
            {
                if (track == null || !Enum.IsDefined(typeof(Direction8), track.direction) ||
                    !seenDirections.Add(track.direction) || !Direction8Utility.IsFinite(track.duration) ||
                    track.duration <= 0f || !Enum.IsDefined(typeof(ActionCurveStyle), track.curves) ||
                    track.poses == null || track.poses.Length < 2)
                    return Fail("Tracks need unique directions, positive durations, and at least two poses.", out error);
                float previous = -1f;
                foreach (var pose in track.poses)
                {
                    if (pose == null || !Direction8Utility.IsFinite(pose.time) ||
                        pose.time < 0f || pose.time > track.duration || pose.time <= previous ||
                        pose.bones == null || pose.rendererOrders == null)
                        return Fail("Pose times must strictly increase from 0 to duration: " + track.direction, out error);
                    previous = pose.time;
                    var bones = new HashSet<string>();
                    foreach (var bone in pose.bones)
                    {
                        if (string.IsNullOrWhiteSpace(bone.bonePath) || !bones.Add(bone.bonePath) ||
                            Array.IndexOf(CharacterRigPaths.All, bone.bonePath) < 0 ||
                            !Direction8Utility.IsFinite(bone.positionPixels) || !Direction8Utility.IsFinite(bone.rotationZ))
                            return Fail("Invalid, unknown, or duplicate bone offset: " + track.direction, out error);
                    }
                    var categories = new HashSet<string>();
                    foreach (var order in pose.rendererOrders)
                        if (string.IsNullOrWhiteSpace(order.category) || !categories.Add(order.category))
                            return Fail("Renderer overrides need unique category names: " + track.direction, out error);
                }
                if (track.poses[0].time != 0f || track.poses[track.poses.Length - 1].time != track.duration)
                    return Fail("First pose must be exactly 0, last exactly duration: " + track.direction, out error);
                if (loop && !SameLoopPose(track.poses[0], track.poses[track.poses.Length - 1]))
                    return Fail("Loop endpoints must match in bones and depth: " + track.direction, out error);
                if (track.markers == null) return Fail("Initialize the marker array.", out error);
                previous = -1f;
                var markers = new HashSet<string>();
                foreach (var marker in track.markers)
                {
                    string identity = marker.name + "@" + marker.time.ToString("R", CultureInfo.InvariantCulture);
                    if (!Direction8Utility.IsFinite(marker.time) || marker.time <= 0f || marker.time >= track.duration ||
                        marker.time < previous || string.IsNullOrWhiteSpace(marker.name) || !markers.Add(identity))
                        return Fail("Markers must be ordered, named, and strictly inside the clip: " + track.direction, out error);
                    previous = marker.time;
                }
            }
            return true;
        }

        public static ActionBoneOffset OffsetAt(ActionPoseKey pose, string bonePath)
        {
            foreach (var entry in pose.bones)
                if (entry.bonePath == bonePath) return entry;
            return new ActionBoneOffset { bonePath = bonePath };
        }

        private static bool SameLoopPose(ActionPoseKey first, ActionPoseKey last)
        {
            foreach (string path in CharacterRigPaths.All)
            {
                ActionBoneOffset a = OffsetAt(first, path), b = OffsetAt(last, path);
                if ((a.positionPixels - b.positionPixels).sqrMagnitude > 0.000001f ||
                    Mathf.Abs(Mathf.DeltaAngle(a.rotationZ, b.rotationZ)) > 0.001f) return false;
            }
            if (first.overrideLimbDepth != last.overrideLimbDepth) return false;
            if (first.overrideLimbDepth && (first.limbDepth.legL != last.limbDepth.legL ||
                first.limbDepth.legR != last.limbDepth.legR || first.limbDepth.armL != last.limbDepth.armL ||
                first.limbDepth.armR != last.limbDepth.armR)) return false;
            if (first.rendererOrders.Length != last.rendererOrders.Length) return false;
            foreach (var order in first.rendererOrders)
            {
                bool found = false;
                foreach (var candidate in last.rendererOrders)
                    if (order.category == candidate.category && order.order == candidate.order) found = true;
                if (!found) return false;
            }
            return true;
        }

        private static bool Fail(string message, out string error) { error = message; return false; }

        private static ActionDirectionTrack[] CreateDirections()
        {
            var result = new ActionDirectionTrack[8];
            for (int i = 0; i < result.Length; i++)
                result[i] = new ActionDirectionTrack { direction = (Direction8)i };
            return result;
        }
    }
}
