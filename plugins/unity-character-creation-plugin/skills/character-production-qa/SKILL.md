---
name: character-production-qa
description: "Audit modular 2.5D Unity character production from canonical artwork through directions, rigging, skeletal clips, equipment, Animator and runtime, and route defects to the correct stage."
---

# character-production-qa

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill character-production-qa --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

For gaps, deformed joints or disappearing parts, run [missing-parts diagnosis](../character-production-orchestrator/references/missing-parts-diagnosis.md) before recommending a fix. Separate source, attachment, skinning, resolver, sorting and transition hypotheses with discriminating checks. Record direction/clip/time, expected silhouette, renderer inventory, revision, exact sampling and captures. A static neutral pose or a few selected keyframes cannot pass moving silhouette QA. Missing rendering evidence keeps that scope blocked/pending; report plugin protocol checks separately from a repaired Unity character.

Input: canonical specs, actual assets/prefab/code, requested direction/action/equipment scope and QA_CHECKLIST.md. Run the Unity gate even for an audit.

1. Build the coverage matrix from the requested scope; distinguish pending, blocked, stale and validated work. Inspect actual evidence paths and revisions. A checkmark or old screenshot does not validate changed assets.
2. Compare artwork to CHARACTER_SPEC.md and DIRECTION_SPEC.md: identity, proportions, palette, overlap, pivots, anatomical sides and native pixel silhouettes. Check all required views and two-character comparisons.
3. Inspect import policy, rig hierarchy/socket bindings, SpriteSkin mesh compatibility and joint ranges. Run joint-skinning-validation for suspect ranges and record failures with reproducible poses.
4. Preview clips, phase timings, loop seams, action markers, front/rear depth, hand grip, direction transitions and terminal poses. Verify walk never swaps/mirrors actual legs; all other actions obey their own grammar.
5. Test equipment changes, controller transitions, action interruption, death hold, movement return, world sorting and multiple instances in Play mode. Include compile/console errors and gameplay marker ownership. Reproduce defects at native pixel scale and the target camera configuration.
6. Update QA_CHECKLIST.md with status, exact artifact revisions, commands/scenes, captures/log paths and observed outcomes. Route artwork gaps to parts, rig issues to rigging, motion to clip authoring, swaps to equipment and state/movement bugs to runtime/controller.

Output: a scoped QA report with pass/block/stale findings and next steps. Only mark a requested release scope validated when its required checks have evidence. Clearly distinguish static file checks from actual Unity compilation, rendered previews and Play mode verification.
