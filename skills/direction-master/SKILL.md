---
name: direction-master
description: "Create consistent neutral direction masters for South, SouthWest, West, NorthWest, North, NorthEast, East, and SouthEast from a validated canonical character. Use for 4- or 8-direction character turnarounds before rig-ready part extraction."
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

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Read `../character-production-orchestrator/references/direction-projection.md` for the eight view and depth conventions.

Requires: validated master artwork and CHARACTER_SPEC.md. Output: neutral direction artwork and DIRECTION_SPEC.md.

1. Read the canonical measurements and inspect the South master. Preserve character identity, palette, body proportions, handedness, asymmetrical clothing, and equipment landmarks.
2. Register all eight canonical names in DIRECTION_SPEC.md, but generate only the requested/needed directions. Default new production to South first. For a 4-direction scope use South, West, North, East; leave diagonals explicitly pending. An all-eight request covers South, SouthWest, West, NorthWest, North, NorthEast, East, SouthEast.
3. Produce neutral views with consistent camera elevation, pixels-per-unit and baseline. Show hidden limbs where plausible; document occlusion instead of forcing identical visibility. Back views must expose back-of-head/clothing rather than reuse the face.
4. Keep anatomical L/R stable: the character's own left/right, even when they appear on the opposite screen side. Do not manufacture another view by mirroring an asymmetric master. Symmetry shortcuts require documented artwork suitability and still preserve physical identities.
5. Compare measured heights, head size, limb lengths, shoulder/pelvis locations, ground line, lighting and silhouette across views. Record per-direction rest poses, pivots, socket orientation and draw-order requirements. Direction-specific projection differences are allowed; unexplained proportion drift is not.
6. Store each source path and revision separately and record validation evidence per direction. Block part extraction for an unvalidated view. Changing a direction master marks its extracted parts, rig calibration and clips stale.

These are neutral source masters, not animation frames. The next stage extracts modular pieces from them.
