---
name: animation-clip-authoring
description: "Author any action on the source skeleton, deterministically bake synchronized pixel frame layers, review/clean every exported pose and register final frame banks."
---

# animation-clip-authoring

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill animation-clip-authoring --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Require validated source direction/calibration/joints. Read [action grammar](../character-production-orchestrator/references/action-grammar.md) and [bake and cleanup](../character-production-orchestrator/references/bake-and-cleanup.md). Accept any stable action ID, not a fixed enum. Record phase grammar, duration, markers, facing lock, interruption/completion and authoring clip paths in ANIMATION_SPEC.md.
2. Author local bone curves against that direction's rest pose using Editor APIs. Melee/cast/jump/roll/death use their own phase sequences; WalkSouth keeps the original five-key 0.5s authoring baseline with real anatomical legs and opposite subtle arms. World motion has a distinct owner.
3. Validate the source clip's joint/curve extrema and intermediate poses. Set proposed output FPS (12 is a starting point, not a guarantee), common duration and explicit sample times. A 0.5s, 12-FPS loop has six samples at 0/12 through 5/12; do not append the repeated endpoint. A final one-shot pose can be held explicitly.
4. Implement/run the actual bake tooling in the connected Editor: deterministic pose sampling, fixed orthographic capture, transparent RGBA, identical canvas/anchor/PPU, isolated body/hair/outfit/item passes and saved raw PNGs. Every layer/item variant samples the exact same pose sequence. Restore source scene/editor state afterward.
5. Inspect and clean EACH final frame at native pixel scale; remove exposed cut seams, correct silhouettes and preserve canonical anatomy/palette. Do not create unrelated generated images per frame as a replacement for a coherent source action. Use supported art tools, preserving raw and cleaned exports separately; unavailable tools are a blocker.
6. Record final sprites, export revision, sampling, render-pass layout, masks, per-frame grip/FX anchors and evidence in FRAME_BANK_SPEC.md. Import/validate final assets through unity-asset-import. Use continuous layered playback and runtime transitions for final QA.

Output: editable authoring AnimationClip, real raw exports, final cleaned synchronized frame banks and evidence. The default runtime plays frames; it does not deform this authoring rig live.
