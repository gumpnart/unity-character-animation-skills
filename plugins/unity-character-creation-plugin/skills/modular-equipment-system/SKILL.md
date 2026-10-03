---
name: modular-equipment-system
description: "Bake equipment on the shared authoring rig and swap synchronized layered pixel frame banks at runtime using item coverage, replacement rules and Sprite Library/Resolver."
---

# modular-equipment-system

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill modular-equipment-system --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md`, `FRAME_BANK_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.


Default production mode: **hybrid-baked-frames**. Read the [hybrid pipeline](../character-production-orchestrator/references/hybrid-pipeline.md). The skeleton is an editable authoring tool; final gameplay uses cleaned synchronized frame layers. Distinguish source rig checks from final frame/runtime checks.

## Stage workflow

1. Read [equipment contract](../character-production-orchestrator/references/equipment-contract.md). Require source sockets/calibration, base pose schedule and item art. Populate EQUIPMENT_SPEC.md with item IDs/slots, bank mappings, coverage, replaced base categories, overlays/masks and compatibility revisions.
2. Equip source clothing/armor on the common authoring rig; attach weapons to real hands. Bake each item using the same action poses/camera/canvas as the base banks. Reuse the authoring clips, but produce and inspect the required item frames; new equipment is not magically covered by existing exports.
3. Define whether an item replaces Clothing_Upper/Lower/Shoes or overlays base art. Hide or mask only the specified covered base regions. Preserve skin/face/hands and a complete valid body; replacing a shirt must not inadvertently remove shorts. Split front/back item/body passes for interleaving rather than placing a whole weapon above/below the body incorrectly.
4. Use SpriteLibrary/SpriteResolver for each render pass with stable labels including appearance/action/direction/frame. One shared playback owner selects all layers for the current index; do not recreate an Animator per item or bone-parent animated runtime gear by default.
5. Validate the whole loadout/action/direction/frame coverage before swapping. Retain the last complete valid loadout on required missing data; optional empty slots have explicit transparent/none selections. A compatible item can reuse the shared banks/timing, not arbitrary mismatched pivots or frame counts.
6. Test armor and a hand-attached source weapon bake during IdleSouth, WalkSouth, one requested action and SW when required. Confirm synchronized hands/grips, clean replacement, correct masks/order and unchanged source action timing. Save real item data/runtime code and evidence.

Output: item bank data, synchronized swap system and a tested baked weapon attachment. Bone sockets are production attachments; runtime FX/projectiles can use recorded per-frame anchors.
