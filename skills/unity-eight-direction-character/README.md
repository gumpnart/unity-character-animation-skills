# Eight-direction Unity RPG character and action animations — Codex skill

A standalone companion to `unity-south-character`. This skill covers **South,
SouthWest, West, NorthWest, North, NorthEast, East, and SouthEast**, with one
shared skeleton and directional equipment appearances.

It also accepts any named character action in natural language, with custom
timing, bone poses, cue markers, and one-shot, loop, or held-pose playback.

**The Unity plugin is mandatory. Every invocation must make a fresh Unity plugin
status/discovery call before character or animation work. If the connection cannot
be established, the skill reports the blocker and stops implementation.**

## Install and use

See [the step-by-step usage guide](references/how-to-use.md) for installation,
ready-to-copy prompts, and what to expect from Codex and Unity.

Copy the entire folder to:

```text
<your-project>/.agents/skills/unity-eight-direction-character/
```

Or use `~/.agents/skills/unity-eight-direction-character/` across projects. Open
a new Codex session with the Unity plugin installed and the target editor connected,
then invoke:

```text
Use $unity-eight-direction-character to implement all eight facing directions
in this Unity project: a shared modular rig, eight idles, eight walks,
directional equipment swapping, and hand-attached weapons.
```

For a custom action:

```text
Use $unity-eight-direction-character to create a two-handed mining animation
in all eight directions: 0.2s anticipation, impact at 0.45s, recovery ending
at 0.9s, with an Impact marker and a hand-attached pickaxe. Return to idle/walk.
```

You can request attacks, spells, gathering, dodges, jumps, reactions, emotes,
or another action. The examples do not limit the accepted action names.

The SOUTH-only skill remains available separately. Use this companion when all
eight directions are intended. See [Unity plugin integration](references/unity-plugin-integration.md)
for the required plugin/CLI/MCP setup and per-invocation handshake. The official
Unity plugin is a separate installation; the ZIP includes its required workflow.

## Included

- Eight individual direction guides and shared skeleton/project specification.
- Full library, equipment, motion, depth, setup, and acceptance documentation.
- C# runtime templates for facing, profiles, equipment, and character control.
- An editor template for capturing authored directional rest poses and generating
  16 looping clips plus a controller without overwriting assets.
- A general action request/specification guide, sample specifications, action
  data model, editor clip/state builder, and integrated runtime playback/markers.
- Mandatory Unity plugin routing, setup/recovery instructions, and the read-only
  `scripts/check-unity-plugin.py` readiness helper.

To upgrade an existing installation, replace the skill folder with this version.
In a Unity project already using its C# templates, update EightDirectionCharacter.cs
and copy all added runtime/editor files. The new partial class file
EightDirectionCharacter.Actions.cs extends the existing component; it is not a
second component to attach.

C# examples use the namespace `RpgEightDirection` to distinguish their types from
the earlier SOUTH templates. Remove the SOUTH driver components from the working
prefab when switching drivers; do not allow both to write animations or sorting.

Templates need a compatible Unity/2D Animation project, directional source art,
Inspector wiring, and visual tuning. They include no finished artwork or prebuilt
prefab. The builder generates starter skeletal curves, not pixel sprite frames.
Unity compilation and visual verification are pending because the packaging
workspace has no Unity editor.
