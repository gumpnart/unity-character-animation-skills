---
name: animator-controller
description: "Configure Unity Animator states and transitions for character idle, locomotion and arbitrary action clips, including attack, cast, hit and death, with explicit parameters, interruption and completion policies."
---

# animator-controller

## Required entry procedure

On EVERY invocation, including design/planning/review, load the installed official Unity plugin's `unity:unity-cli` skill (local name may be `unity-cli`) and make a real fresh readiness/discovery call through its tools. Reading its documentation alone is not a call. Never reuse a previous invocation's readiness result.

CLI bridge example, with the actual absolute project path and THIS skill's name:

```bash
unity status --format json
unity command --caller plugin --skill animator-controller --project-path "/absolute/path/MyUnityGame" --format json
```

Use equivalent installed Unity MCP tools when available; discover their real names and target explicitly. Require successful live target command discovery before production. Inspect status and discovery together: headless Editors may be absent from status; live discovery is decisive. Diagnose Safe Mode or sandbox visibility before declaring an open Editor absent. If the plugin/project/editor is unavailable, report the specific blocker and stop dependent production; no offline implementation fallback. The plugin is separately installed, not embedded or automatically enabled by this pack.

Then read `../character-production-orchestrator/references/production-contract.md` and `../character-production-orchestrator/references/stage-map.md` (for the orchestrator use its own `references/` directory). Find the Unity project root independently of the pack's location. Read its `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md`. These are the durable source of truth; chat requests become recorded revisions. Inspect prerequisites before proceeding. Never overwrite existing specs with blank templates.

## Stage workflow

Apply [missing-parts diagnosis](../character-production-orchestrator/references/missing-parts-diagnosis.md) to transition-only failures. Compare the direction's isolated clip to the real controller crossfade. Check residual scale/visibility/sprite/sorting curves and incompatible directional pose blends, then capture contact/passing switches. Do not certify transitions from standalone clip previews alone.

Requires: validated clip/action records, direction mappings and runtime ownership decisions. Begin with IdleSouth/WalkSouth, then add only requested validated actions.

1. Inspect the existing Animator Controller and parameter users. Define a small parameter contract and write it in ANIMATION_SPEC.md: movement magnitude/direction plus the needed action request/state mechanism. Avoid an enum that permanently limits the action IDs; map arbitrary action data to validated states or an appropriate playable/override strategy if the project already uses one.
2. Make locomotion return from ordinary one-shot actions using the latest movement input. Death can hold its terminal pose and prohibit locomotion. Casting can use explicit hold/release states. Configure loop settings, exit time, transition duration and interruption from each action's specification.
3. Prevent self-retrigger loops, stale triggers and Any State precedence surprises. Define competing action priority (e.g. Hit or Death) from gameplay requirements. Animator, runtime driver and clip markers must share one owner for completion; never have two components independently restore locomotion.
4. Keep equipment selection outside animation state recreation. Avoid blending two incompatible sprite directions/rig calibrations. Set write-defaults/rest restoration policy deliberately and validate return poses; do not rely on implicit defaults.
5. Create/save states and transitions using real Editor APIs through the Unity plugin. Verify each required clip is assigned and expected binding paths exist.
6. Exercise idle → walk → idle; walk → attack → locomotion; cast hold → release; interrupted action; hit; death hold where those actions exist. Verify facing locks, movement input updates and markers. Record controller path, parameter table, transition behavior and evidence in ANIMATION_SPEC.md.

Output: a working controller and its runtime contract. Do not generate placeholder claims for uncreated actions.
