---
name: directional-animation-system
description: "Expand actions and synchronized baked body/equipment frame banks to 4 or 8 directions using per-direction source poses, anchors and occlusion ordering."
---

# directional-animation-system

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill directional-animation-system --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Require a validated base action and requested direction masters/calibrations. Read direction-projection, bake-and-cleanup and layered-frame-playback references. Keep all requested action/direction combinations in the coverage matrix.
2. Reuse semantic action grammar/marker timing but author the appropriate front/side/back/oblique source poses. Preserve anatomical L/R and asymmetric art. Do not mirror one view or reuse South joint/sort assumptions for SW without evidence.
3. Bake all body/item passes for a direction with the same duration, FPS or sample schedule, canvas, anchor and source pose. Validate every final frame and its composite, not only the source rig. Record per-frame order/masks and optional front/back passes.
4. Change direction atomically: validate complete body/loadout frame coverage, preserve normalized gait phase and select the matching frame in ALL layers before rendering. Do not crossfade deforming rigs or independently advance a weapon/armor Animator in the default frame runtime.
5. For action-facing locks retain the action's recorded view until allowed to turn. On missing required data keep the last complete valid view and report the combination; do not make individual body parts disappear.
6. Test locked direction playback and contact/passing transitions separately, especially South/West↔SW. Persist coverage/evidence in DIRECTION_SPEC.md, ANIMATION_SPEC.md and FRAME_BANK_SPEC.md; expand only after the South baseline passes.

Output: complete requested direction/action frame bank mappings and synchronized selection behavior.
