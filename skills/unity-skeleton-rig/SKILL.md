---
name: unity-skeleton-rig
description: "Build or inspect the shared Unity 2D character skeleton, pivots, prefab hierarchy, SpriteSkin bindings, and equipment sockets from imported modular character pieces."
---

# unity-skeleton-rig

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill unity-skeleton-rig --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Requires: imported aligned parts, validated canonical specifications, and the connected target Unity Editor. Use the exact shared skeleton and socket mapping in the production contract.

1. Inspect the prefab/imported rig first and reuse correct bones. Create one character root with Animator, SpriteLibrary, SortingGroup and runtime owner; place the common skeleton under a stable visual root. Keep physics/movement outside local visual bobbing.
2. Set joint pivots from the canonical landmarks and use one hierarchy for every direction. Keep anatomical L/R transform names immutable. Map torso parts onto spine/chest and head/hair onto Head; attach rigid limb pieces to their matching bones.
3. For rigid pixel pieces, prefer renderer transforms or minimally weighted meshes. If using SpriteSkin, generate valid bones/mesh/weights and verify rootBone/boneTransforms and bind poses. A sprite swap with incompatible bone/mesh data is not automatically valid; calibrate each imported direction to the shared rig or use rigid pieces.
4. Add Weapon_R to Hand_R, Weapon_L to Hand_L, HeadEquipment to Head, ChestEquipment and BackEquipment to Chest, WaistEquipment to Pelvis. Document grip and neutral offsets per direction.
5. Save prefab and test scene through Editor APIs. Inspect hierarchy, duplicate bones, transform scale, local axes, renderer grouping, sprite skin bindings and saved asset paths. Record animation binding paths so future clips target real transforms.
6. Record rig revision and inspection evidence in CHARACTER_SPEC.md and DIRECTION_SPEC.md. Send the rig to joint-skinning-validation before authoring clips; structural creation alone is not visual validation.

Output: one shared rigged prefab, six sockets, and a reproducible binding map. Do not build separate independently animated equipment skeletons.
