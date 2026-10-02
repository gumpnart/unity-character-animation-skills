using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using RpgEightDirection;
using Rig = RpgEightDirection.CharacterRigPaths;

// Editor template for one shared, upright, unit-scale rigid skeleton.
public sealed class EightDirectionAnimationBuilder : EditorWindow
{
    private Direction8 captureDirection = Direction8.South;
    private float pixelsPerUnit = 32f;
    private string outputFolder = "Assets/Game/Characters/Animations/EightDirection";
    private static readonly float[] WalkTimes = { 0f, 0.125f, 0.25f, 0.375f, 0.5f };

    [MenuItem("Tools/Eight Direction Character/Animation Builder")]
    private static void Open() { GetWindow<EightDirectionAnimationBuilder>("Eight Directions"); }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Select the character scene instance. Author each direction's " +
            "rest pose, resolver labels, sorting, and sockets, then Capture. This does not " +
            "create directional artwork. Generate after all eight captures are configured.", MessageType.Info);
        captureDirection = (Direction8)EditorGUILayout.EnumPopup("Capture Direction", captureDirection);
        pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit", pixelsPerUnit);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);
        using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
        {
            if (GUILayout.Button("Capture Current Authored Direction")) Run(Capture);
            if (GUILayout.Button("Build 16 Clips and Assign Controller")) Run(Build);
        }
    }

    private static void Run(Action action)
    {
        try { action(); }
        catch (Exception exception) { Debug.LogException(exception); }
    }

    private static EightDirectionCharacter Selected()
    {
        GameObject selected = Selection.activeGameObject;
        if (EditorApplication.isPlaying || selected == null || EditorUtility.IsPersistent(selected))
            throw new InvalidOperationException("Select the root scene instance in edit mode, outside animation preview.");
        var character = selected.GetComponent<EightDirectionCharacter>();
        if (character == null || character.profile == null || character.animator == null)
            throw new InvalidOperationException("Assign EightDirectionCharacter, its profile, and Animator on the root.");
        RequireUprightUnit(selected.transform, true);
        Transform visual = selected.transform.Find("Visual");
        if (visual == null) throw new InvalidOperationException("Missing Visual transform.");
        RequireUprightUnit(visual, false);
        foreach (string path in Rig.All)
        {
            Transform bone = selected.transform.Find(path);
            if (bone == null) throw new InvalidOperationException("Missing bone: " + path);
            RequireUprightUnit(bone, false);
        }
        return character;
    }

    private void Capture()
    {
        EightDirectionCharacter character = Selected();
        DirectionPose pose;
        if (!character.profile.TryGet(captureDirection, out pose))
            throw new InvalidOperationException("Profile needs eight unique direction entries.");
        if (character.bindings == null || character.weaponR == null || character.weaponL == null ||
            character.legL == null || character.legR == null || character.armL == null || character.armR == null)
            throw new InvalidOperationException("Assign resolvers, weapon sockets, and all four limb groups.");
        var defaults = new List<SpriteChoice>();
        var orders = new List<RendererOrder>();
        var categories = new HashSet<string>();
        foreach (var binding in character.bindings)
        {
            if (binding == null || string.IsNullOrWhiteSpace(binding.category) ||
                !categories.Add(binding.category) || binding.resolver == null ||
                binding.resolver.GetCategory() != binding.category ||
                binding.resolver.GetComponent<SpriteRenderer>() == null)
                throw new InvalidOperationException("Configure fixed, unique resolver categories first.");
            defaults.Add(new SpriteChoice { category = binding.category, label = binding.resolver.GetLabel() });
            orders.Add(new RendererOrder { category = binding.category,
                order = binding.resolver.GetComponent<SpriteRenderer>().sortingOrder });
        }
        var bones = new List<BoneRestPose>();
        foreach (string path in Rig.All)
            bones.Add(new BoneRestPose { path = path, position = character.transform.Find(path).localPosition });
        SocketPose right = CaptureSocket(character.weaponR), left = CaptureSocket(character.weaponL);
        Undo.RecordObject(character.profile, "Capture Directional Rest Pose");
        pose.defaults = defaults.ToArray();
        pose.rendererOrders = orders.ToArray();
        pose.bones = bones.ToArray();
        pose.weaponR = right; pose.weaponL = left;
        pose.idleDepth = new LimbOrders(character.legL.sortingOrder, character.legR.sortingOrder,
            character.armL.sortingOrder, character.armR.sortingOrder);
        EditorUtility.SetDirty(character.profile);
        AssetDatabase.SaveAssets();
        Debug.Log("Captured " + captureDirection + ". Verify actual artwork, then tune its motion/depth profile.", character);
    }

    private static SocketPose CaptureSocket(Transform socket)
    {
        Vector3 scale = socket.localScale;
        if (scale.x <= 0f || (scale - Vector3.one * scale.x).sqrMagnitude > 0.000001f ||
            Mathf.Abs(Mathf.DeltaAngle(socket.localEulerAngles.x, 0f)) > 0.01f ||
            Mathf.Abs(Mathf.DeltaAngle(socket.localEulerAngles.y, 0f)) > 0.01f)
            throw new InvalidOperationException("Socket needs positive uniform scale and only Z rotation.");
        return new SocketPose { position = socket.localPosition, rotationZ = socket.localEulerAngles.z, scale = scale.x };
    }

    private void Build()
    {
        EightDirectionCharacter character = Selected();
        if (!Direction8Utility.IsFinite(pixelsPerUnit) || pixelsPerUnit <= 0f)
            throw new InvalidOperationException("Enter the source artwork's positive, finite PPU.");
        string folder = outputFolder.Trim().TrimEnd('/');
        ValidateFolder(folder);
        var clips = new List<AnimationClip>();
        var paths = new List<string>();
        for (int i = 0; i < 8; i++)
        {
            Direction8 direction = (Direction8)i;
            DirectionPose pose;
            string error;
            if (!character.profile.TryGet(direction, out pose))
                throw new InvalidOperationException("Profile needs eight unique direction entries.");
            if (!character.TryValidateDirection(direction, out error))
                throw new InvalidOperationException("Incomplete direction appearance: " + direction + ": " + error);
            var rest = ValidateRest(pose);
            ValidateMotion(pose);
            clips.Add(MakeClip(pose, rest, false, pixelsPerUnit));
            clips.Add(MakeClip(pose, rest, true, pixelsPerUnit));
            paths.Add(folder + "/Idle" + direction + ".anim");
            paths.Add(folder + "/Walk" + direction + ".anim");
        }
        string controllerPath = folder + "/EightDirection.controller";
        foreach (string path in paths) RequireAbsent(path);
        RequireAbsent(controllerPath);
        EnsureFolder(folder);
        for (int i = 0; i < clips.Count; i++) AssetDatabase.CreateAsset(clips[i], paths[i]);
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        AnimatorStateMachine machine = controller.layers[0].stateMachine;
        foreach (var clip in clips)
        {
            AnimatorState state = machine.AddState(clip.name);
            state.motion = clip;
            state.writeDefaultValues = false;
            if (clip.name == "IdleSouth") machine.defaultState = state;
        }
        // The runtime driver selects states directly; no automatic transitions.
        Undo.RecordObject(character.animator, "Assign Eight Direction Controller");
        character.animator.runtimeAnimatorController = controller;
        character.animator.applyRootMotion = false;
        character.animator.updateMode = AnimatorUpdateMode.Normal;
        PrefabUtility.RecordPrefabInstancePropertyModifications(character.animator);
        EditorUtility.SetDirty(character.animator);
        AssetDatabase.SaveAssets();
        Debug.Log("Created 16 clips and EightDirection.controller. Apply/save prefab changes and visually verify every direction.", character);
    }

    private static Dictionary<string, Vector3> ValidateRest(DirectionPose pose)
    {
        var result = new Dictionary<string, Vector3>();
        if (pose.bones == null) throw new InvalidOperationException("Capture bones for " + pose.direction);
        foreach (var bone in pose.bones)
        {
            if (string.IsNullOrWhiteSpace(bone.path) || result.ContainsKey(bone.path) ||
                Array.IndexOf(Rig.All, bone.path) < 0 || !Direction8Utility.IsFinite(bone.position.x) ||
                !Direction8Utility.IsFinite(bone.position.y) || !Direction8Utility.IsFinite(bone.position.z))
                throw new InvalidOperationException("Invalid/duplicate bone pose in " + pose.direction);
            result.Add(bone.path, bone.position);
        }
        if (result.Count != Rig.All.Length) throw new InvalidOperationException("Incomplete bone capture for " + pose.direction);
        return result;
    }

    private static void ValidateMotion(DirectionPose pose)
    {
        if (!Direction8Utility.IsFinite(pose.stridePixels) || !Direction8Utility.IsFinite(pose.bodyBobPixels) ||
            !Direction8Utility.IsFinite(pose.idleBreathPixels) || !Direction8Utility.IsFinite(pose.armSwingRatio) ||
            !Direction8Utility.IsFinite(pose.footLiftPixels) || !Direction8Utility.IsFinite(pose.leftKneeBend) ||
            !Direction8Utility.IsFinite(pose.rightKneeBend) || pose.bodyBobPixels < 0f ||
            pose.idleBreathPixels < 0f || pose.footLiftPixels < 0f || pose.armSwingRatio < 0f || pose.armSwingRatio > 0.5f)
            throw new InvalidOperationException("Invalid motion parameters for " + pose.direction);
    }

    private static AnimationClip MakeClip(DirectionPose pose, Dictionary<string, Vector3> rest,
        bool walking, float ppu)
    {
        var clip = new AnimationClip { name = (walking ? "Walk" : "Idle") + pose.direction, frameRate = 60f };
        float duration = walking ? 0.5f : 1f;
        foreach (var bone in rest)
        {
            for (int axis = 0; axis < 3; axis++)
            {
                string suffix = axis == 0 ? "x" : axis == 1 ? "y" : "z";
                Hold(clip, bone.Key, "m_LocalPosition." + suffix, bone.Value[axis], duration);
                Hold(clip, bone.Key, "localEulerAnglesRaw." + suffix, 0f, duration);
                Hold(clip, bone.Key, "m_LocalScale." + suffix, 1f, duration);
            }
        }
        if (walking)
        {
            Move(clip, rest, Rig.Pelvis, Vector2.up * (pose.bodyBobPixels / ppu), new[] { 0f, 1f, 0f, 1f, 0f });
            Move(clip, rest, Rig.Head, Vector2.down * (pose.bodyBobPixels / ppu), new[] { 0f, 1f, 0f, 1f, 0f });
            Vector2 stride = pose.stridePixels / ppu;
            float[] swing = { 1f, 0f, -1f, 0f, 1f };
            Move(clip, rest, Rig.ThighL, stride, swing);
            Move(clip, rest, Rig.ThighR, -stride, swing);
            Move(clip, rest, Rig.ArmL, -stride * pose.armSwingRatio, swing);
            Move(clip, rest, Rig.ArmR, stride * pose.armSwingRatio, swing);
            Move(clip, rest, Rig.FootL, Vector2.up * (pose.footLiftPixels / ppu), new[] { 0f, 1f, 0f, 0f, 0f });
            Move(clip, rest, Rig.FootR, Vector2.up * (pose.footLiftPixels / ppu), new[] { 0f, 0f, 0f, 1f, 0f });
            Set(clip, Rig.ShinL, typeof(Transform), "localEulerAnglesRaw.z", WalkTimes,
                new[] { 0f, pose.leftKneeBend, 0f, 0f, 0f });
            Set(clip, Rig.ShinR, typeof(Transform), "localEulerAnglesRaw.z", WalkTimes,
                new[] { 0f, 0f, 0f, pose.rightKneeBend, 0f });
            Set(clip, "", typeof(EightDirectionCharacter), "gaitPhase", WalkTimes, new[] { 0f, 0.25f, 0.5f, 0.75f, 1f });
        }
        else
        {
            Set(clip, Rig.SpineUpper, typeof(Transform), "m_LocalPosition.y", new[] { 0f, 0.5f, 1f },
                new[] { rest[Rig.SpineUpper].y, rest[Rig.SpineUpper].y + pose.idleBreathPixels / ppu, rest[Rig.SpineUpper].y });
            Set(clip, Rig.Head, typeof(Transform), "m_LocalPosition.y", new[] { 0f, 0.5f, 1f },
                new[] { rest[Rig.Head].y, rest[Rig.Head].y - pose.idleBreathPixels / ppu, rest[Rig.Head].y });
            Set(clip, "", typeof(EightDirectionCharacter), "gaitPhase", new[] { 0f, 1f }, new[] { 0f, 0f });
        }
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true; settings.startTime = 0f; settings.stopTime = duration;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        return clip;
    }

    private static void Move(AnimationClip clip, Dictionary<string, Vector3> rest,
        string path, Vector2 amplitude, float[] phase)
    {
        var x = new float[5]; var y = new float[5];
        for (int i = 0; i < 5; i++)
        {
            x[i] = rest[path].x + amplitude.x * phase[i];
            y[i] = rest[path].y + amplitude.y * phase[i];
        }
        Set(clip, path, typeof(Transform), "m_LocalPosition.x", WalkTimes, x);
        Set(clip, path, typeof(Transform), "m_LocalPosition.y", WalkTimes, y);
    }

    private static void Hold(AnimationClip clip, string path, string property, float value, float duration)
    {
        Set(clip, path, typeof(Transform), property, new[] { 0f, duration }, new[] { value, value });
    }

    private static void Set(AnimationClip clip, string path, Type type, string property, float[] times, float[] values)
    {
        var keys = new Keyframe[times.Length];
        for (int i = 0; i < keys.Length; i++) keys[i] = new Keyframe(times[i], values[i]);
        var curve = new AnimationCurve(keys);
        for (int i = 0; i < keys.Length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Linear);
        }
        AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, type, property), curve);
    }

    private static void RequireUprightUnit(Transform transform, bool world)
    {
        if (Quaternion.Angle(world ? transform.rotation : transform.localRotation, Quaternion.identity) > 0.01f ||
            ((world ? transform.lossyScale : transform.localScale) - Vector3.one).sqrMagnitude > 0.000001f)
            throw new InvalidOperationException("Starter builder needs upright bone axes and unit scale: " + transform.name +
                ". Orient the sprite children or author clips manually for other bone conventions.");
    }

    private static void RequireAbsent(string path)
    {
        if (AssetDatabase.LoadMainAssetAtPath(path) != null || System.IO.File.Exists(path) ||
            System.IO.Directory.Exists(path) || System.IO.File.Exists(path + ".meta"))
            throw new InvalidOperationException("Output already exists: " + path);
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
