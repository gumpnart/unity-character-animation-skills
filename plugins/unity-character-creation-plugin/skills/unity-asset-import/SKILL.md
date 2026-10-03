---
name: unity-asset-import
description: "Import rig-ready PSD, PSB, or PNG character assets into Unity with compatible 2D Animation and PSD Importer packages, consistent PPU, sprite pivots, and crisp pixel-art settings."
---

# unity-asset-import

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill unity-asset-import --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Use [missing-parts diagnosis](../character-production-orchestrator/references/missing-parts-diagnosis.md) for renderer holes or disappearance. Compare source alpha, sprite rectangle/pivot and generated mesh coverage before blaming animation. Confirm that required opaque/overlap regions survive import; inspect tight geometry and deformation bounds rather than assuming a successful import is a complete sprite.

Requires: validated part inventory and actual Unity project. Use the installed `unity:unity-package-management` and `unity:sprite-editor` skills when their workflows apply, in addition to the mandatory Unity gate.

1. Inspect editor version and installed package versions through the Unity plugin. Verify 2D Animation and PSD Importer support; prefer the supported layered PSB workflow. Do not assume every Unity/PSD Importer version accepts layered PSD equally. Document actual accepted format and convert/export through an appropriate art tool if needed.
2. Record PPU from CHARACTER_SPEC.md and apply it consistently to body and equipment. Use Point filtering, suitable sprite mode, no mipmaps for pixel-art defaults, and compression/settings that preserve the target silhouette. Choose settings from the actual camera/platform needs and record exceptions.
3. Preserve source layer names and inspect generated sprites, rectangles, alpha, pivots, layer-to-transform mapping and texture size. Ensure joint alignment survives the import. PNG pieces can use rigid transform attachment; skeletal mesh use requires proper sprite skinning data, not just a SpriteSkin component.
4. Inspect supported importer rig/character options instead of assuming menu labels or serialized fields. Generate/update assets through Editor APIs exposed by the plugin; never invent GUIDs or hand-edit serialized asset YAML.
5. Reconstruct the neutral character in a test scene and compare to the approved artwork at native scale. Save import evidence and GUID/path inventory in DIRECTION_SPEC.md; record editor/package versions and rendering policy in CHARACTER_SPEC.md.

Output: correctly imported sprites with reproducible import policy. Import success does not imply skeleton or skinning validation. Stop at package/import errors and diagnose them before rigging.
