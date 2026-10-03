---
name: unity-skeleton-rig
description: "Build an authoring-only shared Unity 2D rig, pivots, skin bindings and equipment sockets used to generate reusable poses and bake layered pixel frames."
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

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Require aligned source imports and canonical specs. Reuse a suitable existing rig. Build the exact 21-bone hierarchy and six sockets in the production contract under a dedicated Authoring prefab/scene.
2. Calibrate each direction's rest pose/pivots to its art. Upper arms attach to Shoulder, thighs to Pelvis, child joints follow the parent chain. Preserve anatomical L/R; avoid separate displacement on every limb segment.
3. Prefer rigid pieces/minimal weighting for source pixel silhouettes. SpriteSkin requires actual ordered bone bindings, meshes, weights, rootBone and bind poses; a common hierarchy alone does not validate a new direction's sprite data.
4. Attach Weapon_R/Weapon_L to real hands, and head/chest/back/waist sockets to the recorded bones. Mark source pieces with stable bake render passes, including extra front/back body or clothing passes when an item must interleave with arms/torso.
5. Separate world movement from visual bob and authoring poses. Save the actual prefab, calibration map and bake scene through Editor APIs. Inspect scale, source alignment, hierarchy and binding paths; run joint-skinning-validation.
6. Record the editable rig revision/source paths. The runtime prefab uses baked layer renderers; it does not need this deforming rig or its SpriteSkin components in the default hybrid pipeline. Keep authoring assets for re-baking future equipment/actions.

Output: reusable authoring rig, grip sockets and bake-pass map, followed by visual joint validation.
