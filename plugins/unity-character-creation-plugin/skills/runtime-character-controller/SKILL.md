---
name: runtime-character-controller
description: "Implement movement, facing, one shared frame clock, synchronized body/equipment sprite selection, action markers and ground sorting for baked pixel characters."
---

# runtime-character-controller

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill runtime-character-controller --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Read [layered frame playback](../character-production-orchestrator/references/layered-frame-playback.md). Require imported final banks, Animator/state contract, item coverage and the project input/physics architecture.
2. Put world movement/collision on Character root. Runtime visuals are a small set of flat layer/pass SpriteRenderers with shared canvas/anchor/PPU and SpriteResolvers; no authoring bone deformation or per-limb image rotation in the default hybrid pipeline.
3. One owner supplies action ID, direction, normalized time/index, loadout and speed. Resolve and validate every required layer into a pending frame set, then apply the complete set/order/masks together. Equipment never runs a second independent clock. Retain the last complete valid view on missing required selections.
4. Normalize diagonal input, use a documented dead zone, retain facing when stopped, and obey action-facing locks. Direction changes preserve gait phase. Handle loop wrapping, one-shot final hold/return and variable-speed/frame-rate marker crossings deterministically; gameplay owns hits/resources/knockback.
5. Sort whole characters by ground/feet position via SortingGroup, preserving internal pass order. Do not move whole-character ground sorting with head height or baked body bob. Include multiple characters crossing props and each other.
6. Apply item replacement/overlay/mask rules and recorded grip/FX anchors. Test startup, stops/diagonals, loop seam, action return/interrupt, item swaps mid-cycle, SW changes, low frame rate and scene reload.

Output: actual frame-bank data, movement/playback/swap code and a test scene. Record paths/settings and rendered evidence; source rig clips alone do not satisfy runtime delivery.
