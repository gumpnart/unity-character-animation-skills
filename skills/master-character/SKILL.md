---
name: master-character
description: "Create or refine the canonical master design of a modular 2.5D RPG character before directional artwork, body-part extraction, or Unity rigging. Use for character identity, proportions, pixel style, and base character design."
---

# master-character

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill master-character --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Input: the character brief, available artwork, existing CHARACTER_SPEC.md, and project rendering constraints. For a fresh character this is the first production stage.

1. Inspect the repository and find existing character assets/specifications before creating replacements. Identify the character ID, silhouette, intended camera, style, base outfit, handedness, and equipment needs. Ask only for missing decisions that materially affect identity; record reasonable technical defaults.
2. Create `character-production/CHARACTER_SPEC.md` from the bundled template if absent. Keep identity, measured proportions, resolution, PPU, palette, lighting, anatomical L/R, and source paths together. Unspecified values remain TBD; never invent measured values from unseen artwork.
3. Create or refine ONE neutral base master, normally South: front-facing at a slightly elevated 3/4 RPG angle, both arms and legs readable, limbs near neutral, equipment removable. Use available image tools for new artwork and inspect the result. If tools or source artwork are unavailable, record that blocker; a written description alone is not a completed visual master.
4. Inspect the actual master at native pixel scale. Record landmark coordinates, head/body ratios, limb lengths, pelvis width, joint centers, ground contact, palette and outline rules. Specify source dimensions and how measurements were obtained. Keep these values canonical for all later stages.
5. Record the master asset path, revision, validation evidence, unresolved identity choices, and next stage. Mark ready/validated only when actual artwork satisfies the brief. User design approval is required only if the task explicitly requests an approval gate.

Output: master artwork plus a populated CHARACTER_SPEC.md. Never manufacture a directional set or split/rig the character before this contract is ready. If the user revises proportions, update the revision and mark dependent directions, parts, rigs and clips stale.
