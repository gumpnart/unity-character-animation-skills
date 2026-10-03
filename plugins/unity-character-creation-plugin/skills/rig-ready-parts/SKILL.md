---
name: rig-ready-parts
description: "Separate validated directional character masters into named modular head, torso, arm, and leg pieces with pivots and hidden joint overlap. Use to prepare PSD, PSB, or PNG source artwork for rigid 2D skeletal animation."
---

# rig-ready-parts

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill rig-ready-parts --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

For joint holes or incomplete moving silhouettes, read [missing-parts diagnosis](../character-production-orchestrator/references/missing-parts-diagnosis.md). Validate overlap against the requested direction's rotation extrema, including Southwest; a neutral reassembly or passing South test alone does not validate oblique poses. Preserve complete opaque artwork beneath joints and check that hidden cut-edge outlines/caps do not become visible as seams or bulges during the sweep. Record measured overlap, pivot, allowed angle range and captures per joint/direction.

Requires: CHARACTER_SPEC.md and validated requested neutral directions in DIRECTION_SPEC.md. Read the exact 18-part list in the production contract.

1. Inspect source layers, transparency, dimensions and joint landmarks. Separate Hair_Back, Head, Hair_Front; Torso_Upper, Torso_Lower, Pelvis; and UpperArm, Forearm, Hand, Thigh, Shin, Foot for each anatomical side.
2. Preserve a full-canvas coordinate reference or record each cropped piece's source rectangle and pivot. Do not let independent cropping destroy joint alignment. Use a native layer-capable editor/tool when available; image generation alone must not be claimed to produce an editable layered PSB.
3. Extend hidden artwork under neighboring pieces. Upper arms overlap shoulders; forearms overlap upper arms; hands overlap forearms; thighs overlap pelvis; shins overlap thighs; feet overlap shins. Record overlap depth at the project's resolution.
4. Reconstruct each neutral direction from the exported parts and inspect silhouette, palette and seams at 1:1. Sweep planned joint rotations using rigid pieces to expose gaps. Fix source art before adding deformation.
5. Preserve contour pixels, anatomical identity, consistent pivot units and clear layer names. Do not introduce animation frames, flatten permanently to one body sprite, or mirror another step.
6. Record part paths, source revisions, pivots, overlap measurements and validation status in DIRECTION_SPEC.md; create a part inventory there. Keep source layered files editable and exports reproducible.

Output: 18 aligned parts per requested validated direction, plus an inventory and evidence. Missing artwork or unsupported export tooling remains blocked, not a simulated completion.
