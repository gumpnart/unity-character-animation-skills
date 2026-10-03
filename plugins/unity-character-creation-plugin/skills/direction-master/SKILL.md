---
name: direction-master
description: "Create consistent neutral South and 4/8-direction pixel character masters with shared proportions, camera and ground anchor for layered frame baking."
---

# direction-master

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill direction-master --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Require a validated master and read CHARACTER_SPEC.md. Register South, SouthWest, West, NorthWest, North, NorthEast, East, SouthEast; generate only required views, beginning South for a new character. Read the [direction projection guide](../character-production-orchestrator/references/direction-projection.md).
2. Preserve canonical identity, palette, lighting, anatomical sides, asymmetrical clothing/hair and segment proportions. Create front, side, oblique and back neutral artwork; mirroring an asymmetric character is not a valid turnaround.
3. Use a common bake canvas, native pixel scale, camera elevation and ground anchor. Record intentional projection differences and each view's bone calibration, source pivots, occluded regions and near/far layer ordering in DIRECTION_SPEC.md.
4. Include the base outfit but keep replaceable clothing, hair and equipment separate in the source. Inspect reconstructability and complete hidden joint artwork; do not use a walking screenshot as the only rest reference.
5. Measure and compare landmarks across required views and record source/revision/evidence per view. South validation does not validate SW overlap/pivots. Block part extraction for a missing or unvalidated required view.

Output: neutral source masters and per-direction art/camera/rest/order contract. The eventual runtime uses their baked frame banks, not eight independently live rigs.
