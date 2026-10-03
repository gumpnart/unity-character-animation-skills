---
name: joint-skinning-validation
description: "Validate modular Unity character joints, pivot placement, hidden overlap, rigid limb rotation, SpriteSkin weights, and pixel distortion before or after skeletal animation changes."
---

# joint-skinning-validation

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill joint-skinning-validation --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Follow the [missing-parts diagnosis and evidence gate](../character-production-orchestrator/references/missing-parts-diagnosis.md). For a Southwest defect, compare SW neutral, locked SW playback and South/West↔SW switches before assigning a cause. Inspect joint rotation and motion-curve extrema, parent anchors, source overlap, resolver results and final camera rendering. Record zero unexplained gaps/missing required regions across tested coverage; deliberate occlusion is documented separately. Test intermediate poses and continuous playback, not only valid key poses, before setting validated.

Requires: real imported sprites and rigged prefab. Validate the requested directions, with South as the initial baseline.

1. Capture the neutral pose, then sweep shoulders, elbows, wrists, hips, knees and ankles through the ranges required by the proposed action. Use live Unity poses and inspect native-resolution captures; restore the neutral state afterward.
2. Check transparency gaps, lost silhouette pixels, doubled outlines, pivot orbiting, mesh stretching and unexpected sprite scale. Include occluded limbs and equipment seams, not only the visible front side.
3. Separate source-art, pivot, weight and projection defects. A gap caused by inadequate overlap belongs to rig-ready-parts; a wrong pivot belongs to unity-skeleton-rig/import; a skinning error needs minimal weight correction. Do not hide missing art with extreme deformation or widening the whole limb.
4. For SpriteSkin verify matching bones, bind pose/rest pose, weights and compatible directional sprites. For rigid renderer pieces test rotation boundaries and overlap. Inspect subpixel camera/transform behavior and recorded pixel-art policy.
5. Log each defect with direction, joint, action range, screenshot path, expected/actual result, owner stage and retest evidence in QA_CHECKLIST.md. Update the affected rig/direction validation status in the canonical specs.

Output: joint validation evidence and a clear pass/block/stale status. Return failed joints to the responsible stage; do not mark an untested range validated. Rerun the affected checks when a new action exceeds the original validated ranges.
