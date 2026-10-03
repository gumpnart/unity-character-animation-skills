---
name: character-production-qa
description: "Audit canonical master design, source rig, baked pixel frames, equipment coverage and synchronized runtime playback, diagnosing gaps at the correct production stage."
---

# character-production-qa

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill character-production-qa --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Build requested action/direction/item coverage from the six specs. Validate revisions and actual paths; a planned export or static check is not rendered runtime evidence.
2. Inspect neutral masters/base outfit, source parts/overlap/import/pivots/rig and each direction's source poses. Confirm identity, anatomical sides and complete covered regions appropriate to supported clothing.
3. Inspect ALL final exported frames at native scale and gameplay camera size. Verify alpha, silhouette, joints, palette, stable canvas/ground anchor/PPU, marker/pose schedules, final holds and loop seams. Record raw-vs-cleaned export revisions and preserved source assets.
4. Validate bank coverage and every composited loadout: matching action/direction/time/index in body, armor, hair, weapon; deliberate replacements/masks; front/back interleaving; no leaked base clothing or missing required body regions. Compile success alone cannot pass these checks.
5. Run continuous game playback, contact/passing direction changes, mid-cycle equipment swaps, all required transitions, low frame rate and multiple instances/ground sorting. Use [missing-parts diagnosis](../character-production-orchestrator/references/missing-parts-diagnosis.md) to distinguish source, bake, cleanup and runtime defects.
6. Log exact frame/time/revision, renderer/bank selections, before/after captures, confirmed cause and owner stage in QA_CHECKLIST.md. Zero unexplained gaps/missing required regions in tested scope; deliberate occlusion is recorded. Missing actual rendered evidence keeps the scope blocked/pending.

Output: a scoped artwork-to-runtime QA verdict and routed fixes. Plugin packaging checks do not prove a generated character or exported bank is correct.
