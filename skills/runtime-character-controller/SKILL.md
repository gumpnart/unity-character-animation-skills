---
name: runtime-character-controller
description: "Integrate Unity modular skeletal characters with movement input, 4/8-direction facing, animation action requests, equipment state, 2.5D sorting and gameplay marker events."
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

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Requires: validated prefab, Animator parameter contract, directional mappings and equipment data. Inspect the project's input/physics/gameplay architecture before selecting components.

1. Keep world movement and collision on the gameplay root; keep visual bob and limb animation inside the rig. Choose Rigidbody2D/3D or existing movement as appropriate to the actual 2.5D world. Do not assume 3D physics solely because visuals are described as 2.5D.
2. Normalize diagonal input, map vectors to the requested direction set with a documented dead zone, retain facing at zero input, and honor action-facing locks. Update direction artwork and state consistently. Record screen/world axes and camera mapping.
3. Give one runtime owner responsibility for locomotion, action ID dispatch, completion, interruption and latest input. Expose animation markers as gameplay events; let gameplay validate hits/resources/items. Document gameplay motion versus root-motion and how knockback is applied once.
4. Sort whole characters by feet/world-ground position through SortingGroup and a configurable sorting range/offset; preserve body-part ordering inside each character. Do not let head height, bob, or per-limb depth determine whole-character sorting. Validate multiple characters/props crossing the same ground line.
5. Integrate equipment swaps from modular-equipment-system without replacing the skeleton or rebuilding clips. Missing clips/direction labels must produce useful errors and retain a valid state rather than silently displaying mismatched views.
6. Implement the smallest code that matches the project's architecture, expose essential tuning in serialized data and avoid a generic state framework unless already needed. Test startup, input, diagonals, stops, actions, interrupts, swaps, scene reload and multiple characters.
7. Record code/prefab paths and runtime settings in CHARACTER_SPEC.md; record marker/transition evidence in ANIMATION_SPEC.md and QA_CHECKLIST.md.

Output: runtime integration and a usable test scene, not only pseudocode. Report compilation and Play mode results accurately.
