---
name: unity-asset-import
description: "Import editable rig source assets and baked layered pixel frames into Unity with consistent canvas, pivots, PPU, alpha and supported sprite/package settings."
---

# unity-asset-import

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill unity-asset-import --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Inspect the target Editor/package versions using the Unity plugin. Use official Unity package-management/sprite-editor skills when needed. Verify actual PSD/PSB support; layered PSB is preferred where supported. Never invent importer properties or hand-edit Unity serialized YAML.
2. For authoring source, preserve layer names, pivots, dimensions and alpha. Use consistent PPU, Point filtering and suitable compression/no-mipmap settings for the project. Verify actual source meshes/bind metadata when SpriteSkin is used.
3. For baked runtime PNG frames, read FRAME_BANK_SPEC.md: common canvas, ground anchor, dimensions, PPU, action/direction timing and render-pass layout. Full-canvas frames are the initial default. Trimming/atlasing is allowed only with restored offsets and independently verified pivot alignment.
4. Import final pixel-cleaned frames rather than raw bake outputs when cleanup exists. Preserve transparency and crisp edges; check color/alpha fringes, scaling, texture padding and generated sprite rectangles. Do not pixel-resample each equipment layer differently.
5. Construct SpriteLibrary categories/labels for synchronized frame playback and validate all required body/item selections. Record actual sprite paths/GUIDs and metadata. Optional empty equipment is explicit; missing required body frames is a blocker.
6. Preview imported layered composites at the target camera scale and compare to final art. Record evidence and revisions in specs before setting validated.

Output: calibrated authoring imports and/or final runtime frame sprites according to the stage requested. Import success is not bake or gameplay validation.
