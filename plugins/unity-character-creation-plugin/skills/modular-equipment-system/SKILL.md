---
name: modular-equipment-system
description: "Build modular Unity clothing, armor and hand-held equipment swaps with Sprite Library, Sprite Resolver, common skeleton bindings, and direction-specific equipment sockets and grips."
---

# modular-equipment-system

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill modular-equipment-system --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Use [missing-parts diagnosis](../character-production-orchestrator/references/missing-parts-diagnosis.md) when an equipped character loses body regions. Reproduce the same phase with base body/empty slots, then the item. Validate the whole required item/direction mapping before selection and retain the last valid complete loadout when validation fails; intentional empty equipment must not clear required body sprites. Check the selected item's pivots, SpriteSkin compatibility, grip and occlusion in the failing oblique view.

Requires: validated shared rig, six sockets, direction mappings and imported compatible equipment artwork. Read the equipment contract reference.

1. Create EQUIPMENT_SPEC.md with items and slots, category/label mappings, direction coverage, grip offsets, layered renderer sorting, source assets and rig compatibility requirements. Clothing may replace multiple body categories; rigid weapons use sockets. Multi-part armor can use several renderers with the shared bones.
2. Use SpriteLibrary/SpriteResolver to select the item and direction labels. Choose a stable encoding such as category `UpperArm_L`, label `LeatherArmor_South`; document the exact encoding. A character profile/library contains all resolver categories needed for its active body/equipment appearance. Do not silently drop clothing when a direction changes.
3. Implement a small equipment data model (ScriptableObject is suitable) with item ID, slot, resolver mappings by direction, required coverage, socket local position/rotation/scale, optional prefab and rig revision. Validate all mappings before mutating the visible character; define empty-slot labels explicitly.
4. Attach Weapon_R and Weapon_L to the real hands. Apply direction-specific grip offsets, restore empty slots cleanly, and handle two-handed secondary-hand alignment in the action poses without parenting the root skeleton beneath the weapon.
5. Run a test weapon and chest-equipment swap during IdleSouth, WalkSouth and one action. Confirm no new locomotion clips are needed, limb identities stay fixed, missing labels are reported and directional swaps preserve the loadout.
6. Inspect every supported requested direction, animation contact grip and front/rear sorting. Record actual component wiring, item assets/code paths and evidence in EQUIPMENT_SPEC.md and QA_CHECKLIST.md.

Output: reusable item data, runtime swap code, weapon attachment and a tested sample. Animations belong to the skeleton; equipment has no independent duplicated Walk animation.
