# How to use this skill

## 1. Install it in the Unity project

Download and extract the skill ZIP. Copy its contents into this directory in
the Unity project's root (beside Assets, Packages, and ProjectSettings):

```text
YourUnityProject/
  Assets/
  Packages/
  ProjectSettings/
  .agents/
    skills/
      unity-eight-direction-character/
        SKILL.md
        README.md
        references/
        assets/templates/
        scripts/
```

Keep all references and templates; do not install SKILL.md alone. If upgrading,
replace the prior skill folder with the enhanced version. Open that Unity project
in Codex and start a new session so it discovers the skill.

This installs instructions/templates for Codex. It does not automatically import
the C# templates into Assets or create Unity animation assets. Codex does that
through the required connected Unity plugin workflow.

Install/connect the Unity plugin separately and open the target Unity project.
Follow [Unity plugin integration](unity-plugin-integration.md). On **every use**,
Codex must invoke that plugin and perform fresh live readiness/discovery calls
before continuing, even when the request is a review or action plan.

## 2. Invoke it with a normal request

For an existing character, give Codex the prefab path when known:

```text
Use $unity-eight-direction-character to create a one-handed sword attack
for Assets/Game/Characters/Prefabs/Character8.prefab in all eight directions.
Use the existing Sword_R item and shared bones. Make it 0.7 seconds, strike
at 0.3 with an Impact marker, recover to idle/walk, and allow cancellation.
Implement the action asset, clips, controller states, and playback example.
```

You may be much less specific:

```text
Use $unity-eight-direction-character to create a looping happy dance
for my existing character in all eight directions. Choose suitable timing.
```

For an explicitly limited first action:

```text
Use $unity-eight-direction-character to create a South-facing spell cast
first. Lift both real hands, release the spell at 0.8 seconds, and recover
by 1.2 seconds. Emit CastRelease and retain the equipped staff on Hand_R.
```

Any named character action is accepted; these are examples. You can specify
timing, direction scope, weapon/tool, markers, looping/held behavior, and style,
or let Codex propose reasonable choices. For a completely new character, ask it
to build the shared rig and locomotion foundation before adding the action.

## 3. What Codex should produce

- A concrete action specification with explicit timing and inferred choices.
- The actual Unity plugin call, target project, and readiness result.
- A CharacterActionDefinition asset with authored requested-direction tracks.
- Skeletal Action_<ActionId>_<Direction> clips and states added to the existing
  controller; fixed physical L/R identity and equipment attachments preserved.
- Playback and marker integration suited to the project.
- Validation results and exact paths; missing art/editor capabilities identified.

Codex should inspect the current Unity/package versions and reuse the working
character. A description of the plan alone is not the implementation. It should
generate/import assets through the Unity plugin, then compile and test. When the
plugin/editor is unavailable, it must identify the blocker and setup step, then
stop implementation. It must not silently replace plugin work with generated
standalone code, specifications, or raw Unity asset files.

## 4. Preview in Unity

Open the character test scene and select its root. Assign the resulting action
asset to EightDirectionCharacter.testAction. Enter Play mode, then use the
component context menu Play Test Action. End Test Action releases loops/held
poses; Cancel Test Action works only for interruptible actions.

Inspect motion, both physical hands/legs, marker timing, depth, joint overlap,
and return to idle/walk in each requested direction. Gameplay can use:

```csharp
if (!character.TryPlayAction(myAction, out string error))
    Debug.LogError(error);
```

## 5. Refine it in the same session

```text
Use $unity-eight-direction-character to refine the existing mining action:
reduce torso lean, keep the head steadier, strengthen the anticipation,
and make Hand_L follow the pickaxe shaft more closely in NorthWest.
Preserve the Impact marker at 0.45 seconds and the total 0.9-second duration.
```

Codex should edit the existing action deliberately, preserve the requested cues,
and reverify the changed views. The builder refuses silent file/state overwrites,
so a revision uses existing clip editing or explicit replacement of its generated
assets/states rather than accidentally deleting the locomotion controller.
