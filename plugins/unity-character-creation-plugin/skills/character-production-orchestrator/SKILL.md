---
name: character-production-orchestrator
description: "Coordinate a modular 2.5D Unity character production pipeline from master design to directional artwork, rig-ready parts, import, rigging, arbitrary skeletal actions, equipment and runtime QA. Use to start a character, resume production or select the next stage without skipping prerequisites."
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

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

When resuming a reported defect, first use [missing-parts diagnosis](references/missing-parts-diagnosis.md) with character-production-qa/joint-skinning-validation. Reproduce and classify the affected direction/action before routing a repair. Preserve valid master artwork and unaffected stages; do not regenerate the entire character or expand coverage while the required affected view fails. Require the guide's actual rendered evidence before restoring validated status.

1. Inspect the repository first: Unity project markers, existing character sources, specifications, prefabs, clips and runtime systems. Read `references/production-contract.md`, `references/stage-map.md`, and the current `character-production/*.md` files.
2. If specifications are missing, initialize them from `assets/character-production/` without overwriting existing canonical files. Record the character brief and production scope first, before directional artwork or rigging. Do not create a finished-looking spec with guessed measurements.
3. Resolve scope: South is the first new-character vertical slice; an eight-direction request registers all eight and expands after the South baseline passes. Any action ID is allowed. Keep missing requested combinations pending instead of silently reducing scope.
4. Use the stage map to choose the first incomplete dependency. Begin a new character with master-character. Existing assets can satisfy stages only after real inspection and evidence are recorded; finding files alone is not validation.
5. Explicitly invoke the selected named skill and follow its SKILL.md. Every invocation, including delegated stage invocations, makes a fresh Unity plugin call using its own skill label. Do not simulate use by mentioning a skill name. Do not spawn agents unless the user/project instructions authorize delegation.
6. Persist each stage's outputs, revisions, evidence, blockers and next task in the canonical specs. When upstream art, proportions, rig or action timing changes, mark dependent outputs stale and route to their owner. Resume from files after context loss.
7. Validate IdleSouth → WalkSouth → equipment swap → attached test weapon in a test scene before expanding a new rig broadly. Requested extra actions pass their own grammar/joint/transition checks; no fixed walk grammar constrains them.
8. Finish the requested production scope through character-production-qa. If a prerequisite/tool/editor is missing, record the blocker and explain the smallest recovery step. Do not skip stages or claim completion based on proposed code or nonexistent art.

Output: current canonical specs, completed requested stage outputs, evidence and a durable next-stage record. Orchestration selects work; each specialized skill performs and validates it.
