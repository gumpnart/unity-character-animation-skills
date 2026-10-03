---
name: animator-controller
description: "Build a single character Animator/action state controller that drives synchronized baked frame playback for locomotion and arbitrary actions without separate equipment timelines."
---

# animator-controller

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill animator-controller --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Require final validated action/bank records and FRAME_BANK_SPEC.md. Inspect existing state/input owners. Use a single root Animator for semantic states, direction, speed, action requests and interruption/completion; the frame driver reads its authoritative state/time. Do not animate each layer's sprite independently with competing controllers.
2. Map arbitrary action IDs to actual states/banks without limiting them permanently to a fixed enum. Initial scope is IdleSouth/WalkSouth; add validated requested attack/cast/hit/death/etc. only when their banks exist.
3. Derive the discrete frame index from the shared action clock/sample schedule. Apply state changes to the entire layer set together. Avoid blended frame poses and duplicate marker dispatch during transitions; callbacks, gameplay and playback must have explicit owners.
4. A normal one-shot returns to the latest requested idle/walk. A cast/channel can hold/loop until release. Death can hold the final frame and prevent movement. Configure facing locks, priorities/cancel windows and terminal behavior from ANIMATION_SPEC.md.
5. Equipment selection stays outside state recreation. A direction switch preserves locomotion phase. Verify all bank mappings and missing-data handling before making state transitions visible. Do not leave stale mask/order/layer selections after an action.
6. Save the actual controller/code through Editor APIs and test idle↔walk, action return, direction switch, interrupt, hold/release and death when requested. Log markers crossed by time advancement rather than testing only equality with one frame; include loops, low frame rate and changed playback speed.

Output: one action/state/time contract driving the layered runtime, with controller assets and rendered transition evidence.
