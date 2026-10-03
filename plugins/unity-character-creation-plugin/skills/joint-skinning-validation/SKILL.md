---
name: joint-skinning-validation
description: "Validate authoring joints and final baked pixel silhouettes, then distinguish source overlap, bake defects and layered runtime playback gaps."
---

# joint-skinning-validation

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill joint-skinning-validation --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Read [missing-parts diagnosis](../character-production-orchestrator/references/missing-parts-diagnosis.md) and the hybrid bake/playback guides. Identify whether the defect is visible in authoring poses, raw bake PNGs, final cleaned frames or only the runtime composite.
2. For the authoring rig, compare neutral, locked direction, extreme rotations and intermediate curve poses. Inspect overlap, pivots, parent anchors, segment scale and SpriteSkin bindings/geometry/bounds. Source gaps go to parts; rig/pose errors go to their owner.
3. For the final frame bank, inspect EVERY exported frame at native pixel scale and at the game camera size. Check knees/elbows, cut-edge outlines, silhouette, face/hand/foot identity, alpha fringe and loop seam. Pixel cleanup can correct sampled silhouettes but must be saved as final assets and retain anchors/identity.
4. If final frames are correct but gameplay is not, log the shared frame index, action/direction/item mappings, canvas/pivots, coverage masks and pass sorting at the failure. Do not modify source joints as a fix for a playback synchronization error.
5. For Southwest test neutral, fixed SW loops and South/West↔SW transitions at contact/passing. Required visible regions have zero unexplained gaps/missing parts; deliberate occlusion and natural spaces between limbs are recorded separately.
6. Save before/after renders, offending frame/time, exact revisions, hypothesis/discriminating check/confirmed cause and retest outcome in QA_CHECKLIST.md. Missing actual assets or renders remain blocked/pending.

Output: scoped authoring and baked-composite evidence. Do not certify moving silhouettes from a single neutral image.
