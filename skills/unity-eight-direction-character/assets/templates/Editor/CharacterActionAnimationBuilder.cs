using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using RpgEightDirection;
using Rig = RpgEightDirection.CharacterRigPaths;

// Generates arbitrary timed bone clips from action data. Does not infer choreography.
public sealed class CharacterActionAnimationBuilder : EditorWindow
{
    private CharacterActionDefinition action;
    private float pixelsPerUnit = 32f;
    private string outputFolder = "Assets/Game/Characters/Animations/Actions";

    [MenuItem("Tools/Eight Direction Character/Action Animation Builder")]
    private static void Open() { GetWindow<CharacterActionAnimationBuilder>("Action Animations"); }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Select the Character8 scene root, choose an authored action " +
            "asset, and build. Adds clips/states to its existing locomotion controller. " +
            "Existing files or states are never overwritten.", MessageType.Info);
        action = (CharacterActionDefinition)EditorGUILayout.ObjectField("Action", action,
            typeof(CharacterActionDefinition), false);
        pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit", pixelsPerUnit);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);
        using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
        {
            if (GUILayout.Button("Build Action Clips and Add States"))
            {
                try { Build(); }
                catch (Exception exception) { Debug.LogException(exception); }
            }
        }
    }

    private void Build()
    {
        string error;
        if (action == null) throw new InvalidOperationException("Choose an authored action asset.");
        if (!action.TryValidate(out error)) throw new InvalidOperationException(error);
        if (!Direction8Utility.IsFinite(pixelsPerUnit) || pixelsPerUnit <= 0f)
            throw new InvalidOperationException("Enter the source artwork's positive PPU.");
        GameObject root = Selection.activeGameObject;
        if (EditorApplication.isPlaying || root == null || EditorUtility.IsPersistent(root))
            throw new InvalidOperationException("Select the Character8 root scene instance in edit mode.");
        var character = root.GetComponent<EightDirectionCharacter>();
        if (character == null || character.animator == null || character.profile == null)
            throw new InvalidOperationException("Assign the character, Animator, and directional profile.");
        var controller = character.animator.runtimeAnimatorController as AnimatorController;
        if (controller == null || controller.layers.Length == 0 || controller.layers[0].name != "Base Layer")
            throw new InvalidOperationException("Use a standard AnimatorController with layer 0 named Base Layer.");
        RequireUprightUnit(root.transform, true);
        Transform visual = root.transform.Find("Visual");
        if (visual == null) throw new InvalidOperationException("Missing Visual transform.");
        RequireUprightUnit(visual, false);
        foreach (string path in Rig.All)
        {
            Transform bone = root.transform.Find(path);
            if (bone == null) throw new InvalidOperationException("Missing physical bone: " + path);
            RequireUprightUnit(bone, false);
        }

        string folder = outputFolder.Trim().TrimEnd('/') + "/" + action.actionId;
        ValidateFolder(folder);
        AnimatorStateMachine machine = controller.layers[0].stateMachine;
        var clips = new List<AnimationClip>();
        var paths = new List<string>();
        foreach (var track in action.directions)
        {
            if (!character.TryValidateAction(action, track.direction, out error))
                throw new InvalidOperationException(error);
            DirectionPose profilePose;
            character.profile.TryGet(track.direction, out profilePose);
            var rest = RestPositions(profilePose);
            string stateName = action.StateName(track.direction);
            foreach (var existing in machine.states)
                if (existing.state.name == stateName)
                    throw new InvalidOperationException("State already exists: " + stateName);
            string path = folder + "/" + stateName + ".anim";
            if (AssetDatabase.LoadMainAssetAtPath(path) != null || System.IO.File.Exists(path) ||
                System.IO.Directory.Exists(path) || System.IO.File.Exists(path + ".meta"))
                throw new InvalidOperationException("Output already exists: " + path);
            clips.Add(MakeClip(action, track, rest, pixelsPerUnit));
            paths.Add(path);
        }

        // All input, states, and output collisions were checked before mutation.
        EnsureFolder(folder);
        Undo.RecordObject(machine, "Add Character Action States");
        for (int i = 0; i < clips.Count; i++)
        {
            AssetDatabase.CreateAsset(clips[i], paths[i]);
            AnimatorState state = machine.AddState(clips[i].name);
            state.motion = clips[i]; state.speed = 1f; state.writeDefaultValues = false;
        }
        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();
        Debug.Log("Created " + clips.Count + " skeletal clips for " + action.actionId +
            " and added states to the existing controller. Verify entry, poses, cues, and exit in Unity.", root);
    }

    private static Dictionary<string, Vector3> RestPositions(DirectionPose pose)
    {
        var result = new Dictionary<string, Vector3>();
        if (pose.bones == null) throw new InvalidOperationException("Capture direction bones first.");
        foreach (var bone in pose.bones)
        {
            if (string.IsNullOrWhiteSpace(bone.path) || Array.IndexOf(Rig.All, bone.path) < 0 ||
                result.ContainsKey(bone.path) || !Direction8Utility.IsFinite(bone.position.x) ||
                !Direction8Utility.IsFinite(bone.position.y) || !Direction8Utility.IsFinite(bone.position.z))
                throw new InvalidOperationException("Invalid captured rest bones for " + pose.direction);
            result.Add(bone.path, bone.position);
        }
        if (result.Count != Rig.All.Length)
            throw new InvalidOperationException("Capture all 21 bones for " + pose.direction);
        return result;
    }

    private static AnimationClip MakeClip(CharacterActionDefinition definition, ActionDirectionTrack track,
        Dictionary<string, Vector3> rest, float ppu)
    {
        var clip = new AnimationClip { name = definition.StateName(track.direction), frameRate = 60f };
        var times = new float[track.poses.Length];
        for (int i = 0; i < times.Length; i++) times[i] = track.poses[i].time;
        foreach (var bone in rest)
        {
            var x = new float[times.Length]; var y = new float[times.Length]; var rz = new float[times.Length];
            for (int i = 0; i < times.Length; i++)
            {
                ActionBoneOffset delta = CharacterActionDefinition.OffsetAt(track.poses[i], bone.Key);
                x[i] = bone.Value.x + delta.positionPixels.x / ppu;
                y[i] = bone.Value.y + delta.positionPixels.y / ppu;
                rz[i] = delta.rotationZ;
            }
            Set(clip, bone.Key, typeof(Transform), "m_LocalPosition.x", times, x, track.curves);
            Set(clip, bone.Key, typeof(Transform), "m_LocalPosition.y", times, y, track.curves);
            Set(clip, bone.Key, typeof(Transform), "localEulerAnglesRaw.z", times, rz, track.curves);
            Hold(clip, bone.Key, "m_LocalPosition.z", bone.Value.z, track.duration);
            Hold(clip, bone.Key, "localEulerAnglesRaw.x", 0f, track.duration);
            Hold(clip, bone.Key, "localEulerAnglesRaw.y", 0f, track.duration);
            foreach (string axis in new[] { "x", "y", "z" })
                Hold(clip, bone.Key, "m_LocalScale." + axis, 1f, track.duration);
        }
        Set(clip, "", typeof(EightDirectionCharacter), "gaitPhase", new[] { 0f, track.duration },
            new[] { 0f, 0f }, ActionCurveStyle.Linear);
        var events = new AnimationEvent[track.markers.Length];
        for (int i = 0; i < events.Length; i++)
            events[i] = new AnimationEvent
            {
                time = track.markers[i].time,
                functionName = "OnActionMarker",
                stringParameter = track.markers[i].name,
                messageOptions = SendMessageOptions.RequireReceiver
            };
        AnimationUtility.SetAnimationEvents(clip, events);
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = definition.loop; settings.startTime = 0f; settings.stopTime = track.duration;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        return clip;
    }

    private static void Hold(AnimationClip clip, string path, string property, float value, float duration)
    {
        Set(clip, path, typeof(Transform), property, new[] { 0f, duration }, new[] { value, value }, ActionCurveStyle.Linear);
    }

    private static void Set(AnimationClip clip, string path, Type type, string property,
        float[] times, float[] values, ActionCurveStyle style)
    {
        var keys = new Keyframe[times.Length];
        for (int i = 0; i < keys.Length; i++) keys[i] = new Keyframe(times[i], values[i]);
        var curve = new AnimationCurve(keys);
        AnimationUtility.TangentMode mode = style == ActionCurveStyle.Linear
            ? AnimationUtility.TangentMode.Linear : AnimationUtility.TangentMode.ClampedAuto;
        for (int i = 0; i < keys.Length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(curve, i, mode);
            AnimationUtility.SetKeyRightTangentMode(curve, i, mode);
        }
        AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, type, property), curve);
    }

    private static void RequireUprightUnit(Transform transform, bool world)
    {
        if (Quaternion.Angle(world ? transform.rotation : transform.localRotation, Quaternion.identity) > 0.01f ||
            ((world ? transform.lossyScale : transform.localScale) - Vector3.one).sqrMagnitude > 0.000001f)
            throw new InvalidOperationException("Action builder needs upright rest axes and unit bone scales: " + transform.name +
                ". Adapt/author clips manually for another established rig convention.");
    }

    private static void ValidateFolder(string folder)
    {
        if (!folder.StartsWith("Assets/", StringComparison.Ordinal))
            throw new InvalidOperationException("Output must be below Assets/.");
        foreach (string segment in folder.Split('/'))
            if (string.IsNullOrWhiteSpace(segment) || segment == "." || segment == ".." ||
                segment.IndexOfAny(new[] { '\\', ':', '*', '?', '"', '<', '>', '|' }) >= 0)
                throw new InvalidOperationException("Invalid output folder.");
    }

    private static void EnsureFolder(string folder)
    {
        string[] segments = folder.Split('/'); string parent = segments[0];
        for (int i = 1; i < segments.Length; i++)
        {
            string next = parent + "/" + segments[i];
            if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(parent, segments[i]);
            parent = next;
        }
    }
}
