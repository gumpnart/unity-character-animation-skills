# Unity Character Creation skill pack — v2.0.0

13 Codex workflows for hybrid Unity 2.5D pixel RPG characters. Default `pipeline_mode: hybrid-baked-frames` uses a shared skeleton to author poses, then exports registered layers for pixel cleanup and synchronized runtime frame playback. Supports eight directions and arbitrary action IDs. Start with a complete South slice, then expand requested coverage.

This plugin provides instructions, contracts and templates. The exporter, Unity assets and runtime scripts must be implemented/inspected in the actual target project using its installed Unity tools; they are not bundled finished game components.

## Workflow responsibilities

| Skill | Responsibility |
| --- | --- |
| master-character | Lock identity, measured proportions, neutral stance and simple fitted base outfit |
| direction-master | Calibrate all requested neutral views without identity or anatomical side drift |
| rig-ready-parts | Produce overlapping modular source parts; separate clothing and hidden patches |
| unity-asset-import | Import authoring sources and final registered frame banks correctly |
| unity-skeleton-rig | Shared authoring bones, minimal skinning and six source equipment sockets |
| joint-skinning-validation | Validate source joints and extrema before baking; localize defects |
| animation-clip-authoring | Author any action; run deterministic baking, pixel cleanup and final checks |
| directional-animation-system | Expand source projections and synchronized final action banks to 4/8 views |
| modular-equipment-system | Item source grips, baked coverage, replacement masks and atomic frame swaps |
| animator-controller | One semantic state owner for locomotion/actions and marker policies |
| runtime-character-controller | One clock for all render passes, movement/facing/sorting integration |
| character-production-qa | Source, raw bake, final frame and continuous runtime evidence |
| character-production-orchestrator | Route work, preserve canonical specs, track revisions and finish requested scope |

## Master and source assets

A new master is a neutral South character with balanced feet, both arms/legs readable, fitted sleeveless top, simple shorts and bare feet. Record the accepted outfit and measured native proportions; do not invent dimensions or regenerate an accepted existing master.

Retain 18 source pieces and the shared 21-bone hierarchy described in [production-contract.md](skills/character-production-orchestrator/references/production-contract.md). They are authoring assets, not a requirement for 18 gameplay renderers. Fix joint gaps in overlapping source art rather than excessive mesh deformation. Equipment sockets are Weapon_R, Weapon_L, HeadEquipment, ChestEquipment, BackEquipment and WaistEquipment.

Source WalkSouth keeps .000 Contact A → .125 Passing A → .250 Contact B → .375 Passing B → .500 Contact A. Physical legs never switch identities and arms swing subtly opposite. Five source keys are not five exported frames: at a proposed 12 FPS, a .5-second loop has six unique samples and omits its duplicated endpoint. Choose the final rate based on visual review.

## Runtime and equipment

Final banks share canvas, PPU, ground pivot, exact sample timestamps, action/direction and layout revisions. One clock computes a frame and applies all required Body/Hair/Clothing/Armor/Weapon pass selections together. Sprite Library/Resolver can select the baked sprites; a single root Animator or state machine controls semantics. Independent layer Animators must not drift.

Authoring bones animate the source. Gameplay renders final frames. A new item can reuse source clips but requires its own actual frame coverage. Upper and lower clothing replacements are independent. Use validated occluder masks or split front/back render passes where weapons/hands/torso interleave; simply hiding other layers while exporting can reveal pixels that should be occluded. Validate against the full equipped composite.

Missing required frames/labels retain the last complete valid selection and report the missing bank. Direction/equipment changes preserve normalized action phase. Timeline markers use crossing detection, including low FPS and loops. Optional VFX/hitbox anchors must have a measured frame track.

Read [hybrid-pipeline.md](skills/character-production-orchestrator/references/hybrid-pipeline.md), [master-design.md](skills/character-production-orchestrator/references/master-design.md), [bake-and-cleanup.md](skills/character-production-orchestrator/references/bake-and-cleanup.md), [layered-frame-playback.md](skills/character-production-orchestrator/references/layered-frame-playback.md), and [action-grammar.md](skills/character-production-orchestrator/references/action-grammar.md).

## Unity connection on every invocation

The official Unity plugin must be separately installed and connected. Every skill invocation, including planning and QA, performs real readiness/discovery through actual installed tooling. Refer to [unity-plugin.md](skills/character-production-orchestrator/references/unity-plugin.md). Do not invent tool names or claim an Editor operation ran from a textual plan. Editing this plugin repository does not require a game Editor; running its production skills does.

## Durable state and migration

Initialize the six specs in the target project, preserving populated files: CHARACTER_SPEC, DIRECTION_SPEC, ANIMATION_SPEC, EQUIPMENT_SPEC, FRAME_BANK_SPEC and QA_CHECKLIST. All templates are in [character-production](character-production/) and mirrored inside the orchestrator skill so they remain accessible when installed.

Follow [migration-v2.md](skills/character-production-orchestrator/references/migration-v2.md) to retain existing masters/rigs/clips and replace gameplay visuals after real banks pass QA. Source edits invalidate affected banks. Installation alone does not migrate a game.

For missing parts, compare source → raw → final → runtime before selecting the failing stage. [missing-parts-diagnosis.md](skills/character-production-orchestrator/references/missing-parts-diagnosis.md) separates source overlap/binding defects from masks, crops, import and runtime selection errors.

## Installation choices

Prefer installing the whole plugin through the repository marketplace; see [Desktop instructions](../../docs/INSTALL.th.md). For standalone skills instead, use an existing Unity project:

```bash
python scripts/install_pack.py --project /absolute/path/to/MyUnityGame
```

For an existing standalone installation add `--replace-skills`; canonical specs are preserved and missing files added. `--remove-legacy` removes only the old `unity-eight-direction-character` skill. Do not mix duplicate standalone/plugin copies without reviewing custom edits. Installation is not an asset migration.

```bash
python scripts/validate_pack.py
```
