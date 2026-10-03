---
name: master-character
description: "Define the canonical neutral pixel character, identity, proportions and simple base outfit before directional artwork and rig-assisted frame production."
---

# master-character

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill master-character --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Inspect repository, actual existing character art and specs first. Reuse validated identity/art where possible. Record `pipeline_mode: hybrid-baked-frames`, requested actions/directions, revision and source paths in CHARACTER_SPEC.md. Read the [master design guide](../character-production-orchestrator/references/master-design.md).
2. Create/refine one neutral South master: slightly elevated 3/4 RPG camera, balanced weight on both feet, straight torso, arms slightly away from body, relaxed hands, modest leg separation. A walking pose is an identity reference, not sufficient neutral calibration.
3. Proposed base outfit: plain fitted sleeveless top, simple fitted shorts, bare feet, a defined baseline hairstyle; no armor, hat, weapon, bag or cape. Record defaults as design choices, not measured facts. Existing suitable clothing can be retained. Body identity, removable base clothing and equipment layers are separate contracts.
4. Use available image/art tools to produce/inspect real artwork. Record native canvas, palette, outline/light direction, PPU, ground anchor, anatomical L/R, face/hair identity, measured limb/head/body ratios and source measurement method. Unseen/unmeasured values stay TBD. No invented completed assets.
5. Identify which outfit regions later equipment replaces or overlays. Keep complete covered joint artwork where motion or future supported clothing exposes it. Do not require a nude master; modest base/underlayers and hidden source patches are sufficient for the chosen coverage.
6. Save actual asset/revision/evidence and unresolved decisions. Mark validated only after native-scale inspection meets the brief; explicit human approval is needed only when the task requests it. Changes to proportions invalidate dependent art/rig/baked banks. Do not skip ahead to eight-direction animation.

Output: a canonical neutral master and durable CHARACTER_SPEC.md. This master locks identity; production frame cleanup may refine poses while preserving it.
