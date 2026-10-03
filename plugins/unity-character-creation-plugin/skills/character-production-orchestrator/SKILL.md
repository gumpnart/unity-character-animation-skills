---
name: character-production-orchestrator
description: "Coordinate the hybrid Unity pixel character pipeline from neutral master and source rig to baked cleaned frame layers, modular equipment and synchronized gameplay."
---

# character-production-orchestrator

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill character-production-orchestrator --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Inspect the repository first and read production-contract, stage-map, hybrid-pipeline, master-design and all six canonical specs. Find the actual Unity game project independently of the plugin's location.
2. Initialize missing specs from `assets/character-production/` without overwriting populated files. For new work record `pipeline_mode: hybrid-baked-frames`. Preserve existing identity and source rigs. For projects using v1.x read [migration](references/migration-v2.md), record the pipeline revision and invalidate affected runtime/frame acceptance, not valid artwork indiscriminately.
3. Start a new character with a neutral master and simple base outfit. Register the user's requested direction/action/item scope; South remains the first small vertical slice even for eventual all-eight coverage. Any named action is supported.
4. Select the first incomplete dependency and explicitly invoke its named skill. Every invocation makes its own fresh Unity plugin call. Do not invent asset completion or skip from source clips to validated runtime; real bake outputs, pixel cleanup review and final frame-bank imports are required gates. No agent delegation unless user/project instructions authorize it.
5. Prove master → South source parts/import/rig/joints → author Idle/Walk → bake/clean/import frame layers → shared-clock runtime → outfit replacement/weapon bank swap → QA. Then expand requested directions/actions/items using the same production contracts.
6. Update revisions, evidence, blockers and next work in the six specs. A source pose change invalidates its baked banks; an item change can require re-baking that item without recreating the shared clips. Existing specs gain fields rather than blank-template replacement.
7. For a defect, compare source pose, raw bake, final cleaned frame and runtime composite first. Preserve valid master art and route the confirmed failing stage. Finish requested coverage through character-production-qa with actual rendered evidence.

Output: durable production state and completed requested assets/runtime. This plugin defines a workflow; installing it does not bake assets or repair a game automatically.
