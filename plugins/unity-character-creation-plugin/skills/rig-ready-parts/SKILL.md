---
name: rig-ready-parts
description: "Prepare aligned modular source pieces with hidden overlap and removable clothing layers for rig-assisted pixel animation baking and final silhouette cleanup."
---

# rig-ready-parts

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill rig-ready-parts --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Require validated requested neutral masters. Use the 18 authoring part names in the production contract; add separate base-clothing/hair/equipment source layers as needed. These source pieces are not the required runtime renderer layout.
2. Preserve full-canvas coordinates or record each crop rectangle, offset and pivot. Export real editable PSB/PSD/PNG through supported art tools; an image generation result alone is not a layered source file.
3. Extend opaque source artwork beneath shoulders, elbows, wrists, hips, knees and ankles for each direction's required rotations. Keep hidden cut outlines/caps from emerging as seams/bulges. Record overlap, pivot and allowed ranges in DIRECTION_SPEC.md.
4. Reconstruct the neutral master and sweep extreme planned poses. For SW gaps follow [missing-parts diagnosis](../character-production-orchestrator/references/missing-parts-diagnosis.md). Fix source holes and wrong pivots before baking; do not mask them with excessive mesh stretch.
5. Tag every source piece with a render-pass/coverage role. Clothing can replace base outfit categories or overlay them; skin patches under seams remain complete for supported outfits. The final runtime may flatten several source pieces into Body while retaining front/back passes needed for interleaving.

Output: aligned editable source parts and an inventory suitable for authoring and layer-isolated baking, with native-scale reconstruction evidence.
