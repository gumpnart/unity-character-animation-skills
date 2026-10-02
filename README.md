# Unity Character Animation Skills for Codex

Build modular 2.5D RPG characters and custom skeletal action animations in Unity
using a shared rig, Unity 2D Animation, PSD Importer, Sprite Library, Sprite
Resolver, and Animator.

## Included skill

[unity-eight-direction-character](skills/unity-eight-direction-character/SKILL.md)
supports South, SouthWest, West, NorthWest, North, NorthEast, East, and SouthEast.
It accepts any named character action with custom poses/timing, markers, looping,
one-shot or held-pose playback, directional depth, and hand-attached equipment.

**Every invocation requires a fresh Unity plugin call and target-editor readiness
check.** If the plugin cannot connect, the skill reports the blocker and stops
implementation. The official Unity plugin is installed separately; this repository
contains the skill, integration instructions, and a readiness helper.

## Install

Clone this repository, then copy the skill directory into your Unity project:

```bash
git clone https://github.com/gumpnart/unity-character-animation-skills.git
mkdir -p /path/to/YourUnityProject/.agents/skills
cp -R unity-character-animation-skills/skills/unity-eight-direction-character /path/to/YourUnityProject/.agents/skills/
```

Replace the project path with your own. Keep all references, templates, and
scripts. Open a new Codex session in that project after installation.

Install/connect the Unity plugin using the
[integration instructions](skills/unity-eight-direction-character/references/unity-plugin-integration.md).
The documented CLI/Pipeline bridge supports Unity 6.0+. Reuse a compatible
existing project and editor; do not silently upgrade an older project.

## Use

```text
Use $unity-eight-direction-character to create a two-handed mining animation
for my existing character in all eight directions. Use Pickaxe_R on Hand_R,
align Hand_L with its shaft, impact at 0.45 seconds, recover by 0.9 seconds,
emit an Impact marker, and return to idle or walk.
```

You can also request an action such as a looping celebration dance and let
Codex propose suitable timing. For a new character, start with a master design,
modular directional artwork, and a shared rig before authoring actions.

See the [usage walkthrough](skills/unity-eight-direction-character/references/how-to-use.md),
[action authoring guide](skills/unity-eight-direction-character/references/action-authoring.md),
and [example specifications](skills/unity-eight-direction-character/references/action-examples.md).

## What is provided

- Exact shared skeleton, prefab/part hierarchy, and eight directional guides.
- Sprite Library, equipment data, weapon sockets, motion/depth, and setup guidance.
- C# runtime and editor templates for locomotion and arbitrary named actions.
- Mandatory Unity plugin routing and a read-only CLI readiness helper.

This repository includes no finished character artwork or prebuilt Unity prefab.
Existing rigged characters can be reused. Source templates and packaging were
checked, including mocked readiness-helper cases; Unity compilation, generated
asset behavior, and visual play-mode verification remain pending in a real project.
