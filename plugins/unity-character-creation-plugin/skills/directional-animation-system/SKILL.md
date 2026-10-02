---
name: directional-animation-system
description: "Expand validated skeletal character actions to 4 or 8 directions with stable anatomical limbs, common phase timing, direction-specific poses and draw ordering, and synchronized facing changes."
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

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Read `../character-production-orchestrator/references/direction-projection.md` before adapting poses and renderer depth.

Requires: a validated base action in ANIMATION_SPEC.md, validated requested direction masters/rig calibrations, and DIRECTION_SPEC.md.

1. Read requested scope and register the exact canonical direction names. For an all-eight request cover South, SouthWest, West, NorthWest, North, NorthEast, East, SouthEast; do not mark missing views complete. Keep South-first validation as the initial checkpoint for a new character.
2. Preserve action semantics, markers, phase durations and actual L/R identities. Author per-direction rest-relative bone poses and sprite labels, with depth swing for South/North, side projection for East/West and blended projection for diagonals. Sharing timing does not imply copying identical curves across views.
3. Define each direction's neutral sort order, transient action sort changes and socket/grip orientation in DIRECTION_SPEC.md. Back/side occlusion and clothing asymmetry must match the neutral masters. No negative-scale mirroring to fake a direction or the opposite step.
4. Use explicit action/direction clip mappings for the first implementation; choose a blend tree only when compatible rigs/curves and visual blending have been verified. Apply direction library selection and clip state on the same update; preserve current gait phase during locomotion changes. Lock facing for an action when its spec requires it.
5. Preview direction changes at rest, contact, passing and action markers. Verify no identity jumps, pose pops, flipped grips, stale sorting or wrong artwork. Align phase markers for variants and document justified variations.
6. Save mapping/coverage and evidence in ANIMATION_SPEC.md and DIRECTION_SPEC.md. Missing required action/direction combinations block the requested release scope, while unrequested directions stay pending.

Output: validated per-direction action clips and deterministic facing/clip/art selection contract. Delegate actual clip creation to animation-clip-authoring through explicit invocation so its Unity gate also runs.
