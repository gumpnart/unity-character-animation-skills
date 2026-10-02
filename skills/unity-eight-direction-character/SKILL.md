---
name: unity-eight-direction-character
description: Create any requested skeletal character action animation for eight-direction 2.5D RPGs. Requires calling the Unity plugin on every invocation before proceeding. Use for Unity rigs, idle/walk, attacks, casting, gathering, dodges, jumps, reactions, emotes, and custom timed actions with markers, loops, directional depth, shared equipment/bones, 2D Animation, PSD Importer, Sprite Library, Sprite Resolver, and Animator.
---

# Eight-direction Unity character and custom actions

## Mandatory Unity plugin call — every invocation

This skill REQUIRES the installed Unity plugin. **Every time this skill is invoked,
explicitly or implicitly, first invoke the Unity plugin and make a real Unity
readiness/discovery call before proceeding with character or animation work.**
This applies to creation, refinement, debugging, reviews, and planning requests.
Do not treat the plugin as optional or reuse a previous turn's connection result.

1. Load/invoke the installed Unity plugin's `unity:unity-cli` skill (`unity-cli`
   in local installations). Use its current instructions and callable tools.
2. Call the Unity plugin's actual editor status/discovery tools. If it exposes MCP
   tools, discover and call the available Unity tools with the target project.
   Otherwise use the plugin's documented CLI bridge from the actual project cwd:

   ```bash
   unity status --format json
   unity command --caller plugin --skill unity-eight-direction-character --project-path "/absolute/path/to/project" --format json
   ```

   `unity status` alone can omit a resident headless Editor; a successful live
   command discovery against the explicit project establishes reachability.
   The bundled `scripts/check-unity-plugin.py` performs these CLI calls and checks
   their results; it is a convenience, not a replacement Unity plugin.
3. Confirm the requested project and an editor that actually serves commands.
   Use Unity plugin commands/MCP for hierarchy inspection, C# editor execution,
   prefab/assets, Animator clips/states, scene changes, saving, and verification.
   Discover actual editor command names; do not invent tool names. Label every
   `unity command` call `--caller plugin --skill unity-eight-direction-character`.
4. If the plugin, executable/tools, project target, or editor connection is missing,
   report the exact blocker and the setup/recovery step. Authorized bootstrap
   operations may use the Unity plugin to establish the connection. **If readiness
   cannot be established, stop character/animation implementation; do not switch
   to a code-only, specification-only, or manually serialized asset fallback.**

Reading Unity skill docs, printing a command without executing it, or mentioning
the plugin in prose does not satisfy the call requirement. Never fabricate a
plugin call, connected editor, or verification result. Record the actual plugin
call/project and outcome in the completion report.

Read [Unity plugin integration](references/unity-plugin-integration.md) for setup,
safe-mode/sandbox recovery, package management, and execution details. This rule
governs Codex's skill workflow; it does not call plugins from gameplay Update.

Implement all eight facing directions using one shared physical skeleton and
equipment identity. This companion extends the SOUTH handoff to all directions
because the user explicitly requested that expansion. SOUTH is a useful first
verification milestone, not the final scope of this skill. Accept any named
action request; the action list is open-ended, not a fixed enum or catalog.

## Inspect first

- Read repository instructions, Unity version, installed package versions, source
  PSD/PSB art, prefabs, libraries, and existing controllers. Required capabilities
  come from `com.unity.2d.animation` and `com.unity.2d.psdimporter`; choose versions
  supported by the current project. Confirm APIs against installed packages.
- After the mandatory plugin call, use Unity plugin editor tooling to generate
  assets; do not invent serialized YAML or bypass a reachable editor with raw edits.
- Reuse compatible artwork and bones. If assets are missing, create concrete code,
  asset specifications, and setup instructions, and identify pending editor/artwork
  work through the connected plugin workflow. Missing artwork and missing plugin
  readiness are distinct: unavailable plugin readiness blocks implementation.
  Do not claim a running character from an uncompiled template.
- If migrating the SOUTH template, keep the exact bone paths and original physical
  L/R identities. Replace its runtime depth/locomotion/equipment components when
  using the eight-direction driver, to avoid competing writers.

## Required constraints

- The base character supports South, SouthWest, West, NorthWest, North,
  NorthEast, East, SouthEast. Actions default to these eight directions; an explicit
  requested subset is valid. Use these exact suffixes in code and clip names.
- One Transform skeleton owns motion. L = physical Leg A, R = physical Leg B,
  permanently. Do not exchange sprites between physical limb categories, exchange
  bone references, mirror the opposite contact, flip the character, or use negative
  scales to synthesize a step or direction.
- Author real directional artwork. Direction selection changes labels and motion
  profiles; it does not rotate SOUTH art into an adequate eight-direction character.
- SpriteLibrary/SpriteResolver changes appearance, not skeletal motion. Animator
  clips must never key sprite selections, equipment offsets, or library hashes.
- Use rigid bone-parented pixel-art pieces first. Limit skin deformation and weights
  to necessary joints. Fix gaps by improving hidden source-art overlap. Skinned
  swaps additionally require compatible meshes, weights, bone maps, and bind poses.
- Keep SOUTH depth-based and both limbs readable. North reverses the screen-depth
  meaning of a forward stride. Side views have near/far limbs. Diagonals combine
  horizontal progression and screen depth; their layering needs authored profiles.
- For walking, arms counter-swing relative to legs in every direction. Action poses
  follow the requested behavior rather than inheriting walk's counter-swing rules.
  Weapon_R always follows
  Hand_R and Weapon_L always follows Hand_L, even when the visible hand is far away.
- Keep world movement separate from gait. Accept movement already projected into
  screen/camera space; map world axes explicitly for the actual 2.5D camera.
- Turning must validate all body/equipment labels, sorting configuration, and target
  animation before mutation. Reject an incomplete turn as a whole and retain the
  previous visual facing and equipment. Missing data must yield an actionable error.
- Retain the last facing while idle. Use a dead zone and sector hysteresis. Preserve
  normalized gait phase when turning while walking. Start with instant state changes
  and coherent directional artwork; blending poses is a later refinement.

## Work sequence

For action work in an existing character project, inspect and reuse the current
rig/controller first. Implement only the requested action and missing dependencies;
do not rebuild working locomotion or generate unrelated action examples.

1. Read [shared architecture](references/architecture.md) and
   [directional design](references/directional-design.md). Build/retain the exact
   shared skeleton and six sockets; prepare eight part sets and compatible pivots.
2. Read [animation and depth](references/animation-and-depth.md), then the relevant
   direction guides below. Author rest poses, idle/walk curves, and depth orders.
3. Read [equipment and library](references/equipment-and-library.md). Use fixed
   categories, directional labels, complete multipart variants, and stable item IDs.
4. Adapt the namespaced C# templates in `assets/templates/Runtime/`. They provide
   direction quantization, profile/item data, validated whole-character appearance
   application, gait-preserving Animator selection, and LateUpdate limb depth.
5. Optionally adapt `assets/templates/Editor/EightDirectionAnimationBuilder.cs`:
   capture each authored direction rest pose, then generate 16 clips and a controller.
   It requires the documented paths, upright bone axes, and unit scales. Its small
   starter curves require visual tuning; it does not create art or automatically rig.
6. Follow [setup and verification](references/setup-and-verification.md). Confirm
   every direction's contact poses, equipment attachment, missing-variant behavior,
   idle retention, sector boundaries, and turns during gait before calling it done.
7. For any action request, read [action authoring](references/action-authoring.md)
   and [action playback](references/action-playback.md). Turn the description into
   a concrete action specification, author direction-specific timed bone poses,
   generate clips/states with the action builder, and verify the requested motion.

## Accept any action

The user can request an animation in ordinary language, for example "make a
two-handed mining swing", "a 1.2-second healing spell", or "a looping cheerful
dance". Use [the request template](references/action-request-template.md) to
capture action ID, intent, direction scope, timing, poses, loop/finish behavior,
equipment needs, markers, and movement policy. Infer sensible reversible defaults
and state them; ask only when missing information materially blocks the result.

Default to all eight directions, a named full-body one-shot, locked visual facing,
linear bone curves, and return to current idle/walk when finished. Duration and
poses come from the action's intent, not walk's 0.5-second cycle. A looping dance,
held death pose, or explicitly requested direction subset overrides those defaults.
Do not create unrelated actions just because they appear in examples.

Use one `CharacterActionDefinition` asset per requested action and one directional
track per supported facing. Each track has arbitrary increasing timestamps,
sparse rest-relative bone offsets, optional depth overrides, and named cue markers.
`assets/templates/Editor/CharacterActionAnimationBuilder.cs` generates full-property
skeletal clips and adds action states to the existing controller without replacing
locomotion. Actions use `Action_<ActionId>_<Direction>` state/clip names.

The enhanced EightDirectionCharacter template also owns action playback. It keeps
locomotion from overriding a running action, supports completion/hold/loop/cancel,
forwards markers, and restores depth and the latest requested idle/walk state.
Do not add a second Animator owner. Visual jumps/dodges do not implement world
movement, collisions, damage, inventory, or spell logic; expose cues and integrate
those behaviors only within the actual requested scope.

## Direction guides

| Facing | Guide | Screen movement |
|---|---|---|
| South | [South](references/directions/south.md) | down |
| SouthWest | [SouthWest](references/directions/south-west.md) | down-left |
| West | [West](references/directions/west.md) | left |
| NorthWest | [NorthWest](references/directions/north-west.md) | up-left |
| North | [North](references/directions/north.md) | up |
| NorthEast | [NorthEast](references/directions/north-east.md) | up-right |
| East | [East](references/directions/east.md) | right |
| SouthEast | [SouthEast](references/directions/south-east.md) | down-right |

## Shared gait timing

All Walk<Direction> clips initially use a 0.5-second loop. This timing applies to
walking only; custom actions use their own durations and any number of poses:

| Seconds | Pose | Forward leg | Forward arm | Body bob |
|---|---|---|---|---|
| 0.000 | Contact A | L | R | 0 px |
| 0.125 | Passing A | L moving back, R moving forward | counter-swing | +1 px |
| 0.250 | Contact B | R | L | 0 px |
| 0.375 | Passing B | R moving back, L moving forward | counter-swing | +1 px |
| 0.500 | Contact A | L | R | 0 px |

Forward means along the facing direction. It means camera-near for SOUTH,
camera-far for NORTH, and must not be confused with the camera-side limb in side
views. Convert pixel offsets to units with actual PPU. Head compensation keeps
the head comparatively stable. Endpoints match; interpolate actual bone motion.

## Deliverables

When building the complete character, produce the original twelve deliverables
expanded to eight directions:

1. Unity folders for shared rig, eight source-art sets, libraries, items, and clips.
2. Shared character prefab with resolver parts, limb groups, and six sockets.
3. Exact physical bone hierarchy from the handoff, with persistent L/R identity.
4. Directional Sprite Library category/label schema.
5. Item model with eight appearance/grip variants and shared item identity.
6. Animator Controller with 16 locomotion states plus requested action states.
7. Eight Idle<Direction> skeletal clips.
8. Eight Walk<Direction> clips with the shared five-pose timing.
9. Validated equipment swapping and coherent direction switching.
10. Weapon_R/Weapon_L hand attachment and directional grip calibration.
11. Import, Inspector wiring, authoring, compile, and play-mode instructions.
12. Maintainable expansion guidance for characters, equipment, and custom actions.

For action work, additionally deliver the action specification/asset, requested
directional clips, Animator states, playback calls, named markers, and visual
checks for entry, key poses, equipment grip, exit/loop, cancellation, and depth.

Return exact file paths, the Unity plugin call/project/readiness result, and
verification evidence. Distinguish templates, generated assets, Unity compilation,
and visual results. Missing directional art can leave visual checks pending; a
missing Unity plugin/connection must be reported as a blocked invocation, not
replaced by standalone implementation.
