---
name: animation-clip-authoring
description: "Author any named Unity skeletal character action animation, including locomotion, jump, dodge, attacks, casting, hit, death, interaction and emotes, using action-specific pose grammar, timing, markers and loop/one-shot behavior."
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

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Requires: validated master, imported/rigged target direction, passing joint checks, and actual transform bindings. Read the action grammar reference in `../character-production-orchestrator/references/action-grammar.md`.

1. Interpret the requested action without limiting it to a fixed enum or predefined list. Find or add its record in ANIMATION_SPEC.md: action ID, direction, duration, playback, grammar, phase timings, poses, markers, root-motion policy, facing lock, interrupt rules and completion behavior. Keep unspecified gameplay timing provisional and label assumptions.
2. Choose phases appropriate to the action. Melee: anticipation → acceleration → contact → follow-through → recovery. Cast: anticipation → channel/hold → release → recovery. A loop, jump, bow, roll or death needs its own grammar; never force all actions into the walk cycle.
3. Author local bone-transform curves in Unity AnimationClip assets through supported Editor APIs/plugin commands. Capture direction-specific rest values first; compose offsets against them. Write complete intended bindings/rest values so returning from a previous action does not leave stale rotations or poses. Avoid animation of world movement unless root motion is explicitly chosen.
4. Use rigid UpperArm/Forearm/Hand and Thigh/Shin/Foot motion with minimal mesh deformation. Keep hand grip, hidden overlaps and readable silhouettes. Animate sorting only for the intended action/direction; do not silently inherit locomotion front/rear ordering.
5. For WalkSouth use the exact five keys and leg/arm depth rules in the grammar reference. For other actions use their specified phase counts and timings. Markers communicate contact/release/etc. to runtime; an animation marker alone must not apply damage or item effects.
6. Preview the entire clip, phase boundaries, loop seam or terminal pose, weapon alignment, sorting changes and interrupt/return pose. Record actual clip paths, binding checks and evidence in ANIMATION_SPEC.md and QA_CHECKLIST.md.

Output: the requested skeletal clip(s), complete action specification and evidence. Stop on missing art/rig prerequisites. Ask directional-animation-system to expand a validated base action; do not create frame-by-frame sprite sheets.
