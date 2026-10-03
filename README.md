# Unity Character Creation — Codex Desktop Plugin

Codex plugin **`unity-character-creation-plugin`**, version **1.0.1**, containing the complete **13-skill** character production pack. It starts from a canonical master character and supports eight directions, arbitrary skeletal actions, equipment swapping, Animator/runtime integration and QA.

แพ็กนี้ติดตั้งเป็น Codex plugin ได้ทั้งชุด พร้อม 13 skills และ specification templates ที่ใช้ร่วมกัน ตั้งแต่ Master Character จนถึง runtime และ QA

## Install in Codex Desktop

1. Clone this repository into a permanent local folder on the computer running Codex Desktop.
2. Generate the **View plugin** link using the local repository path:

   ```bash
   git clone https://github.com/gumpnart/unity-character-creation-plugin.git
   cd unity-character-creation-plugin
   python scripts/open_plugin.py
   ```

3. Open the printed `codex://plugins/...` link on the same computer running Codex Desktop, or run `python scripts/open_plugin.py --open`. In the plugin detail page, choose **Install** and enable it. The marketplace entry makes the plugin available; enabling/installing happens in the app.
4. Install/connect the official **Unity** plugin separately and open the target Unity project. Open that game project in Codex and start a new thread with the prompt below.

See the [Thai installation walkthrough](docs/INSTALL.th.md). The repository marketplace is `.agents/plugins/marketplace.json`; it points to `./plugins/unity-character-creation-plugin`. Its paths remain valid after cloning. The helper produces a deeplink containing YOUR actual local marketplace path; a cloud workspace path cannot locate files on your desktop.

For an existing clone of the renamed repository, update its origin:

```bash
git remote set-url origin https://github.com/gumpnart/unity-character-creation-plugin.git
git remote get-url origin
```

The plugin identifier and directory are now `unity-character-creation-plugin`. If you installed the earlier `unity-2-5d-character` plugin, open the current marketplace and install/enable the new identifier before selecting its skills.

## Start using the plugin

```text
Use the character-production-orchestrator skill from the unity-character-creation-plugin plugin.
We are starting a new modular 2.5D RPG character.
Start from the master-character stage.
Inspect the repository first.
Create the character production specification before directional artwork or rigging.
Do not skip stages.
```

```text
Use the animation-clip-authoring skill from the unity-character-creation-plugin plugin.
Create a sword light-attack animation for South.
Use anticipation → acceleration → contact → follow-through → recovery.
```

Codex may display plugin-qualified skill names. Select the skill contributed by `unity-character-creation-plugin`; every SKILL.md retains the original stage name. You can request custom actions such as mining, fishing, climbing or dance; authoring is not limited to a fixed action list or walk grammar.

## Included workflows

1. master-character
2. direction-master
3. rig-ready-parts
4. unity-asset-import
5. unity-skeleton-rig
6. joint-skinning-validation
7. animation-clip-authoring
8. directional-animation-system
9. modular-equipment-system
10. animator-controller
11. runtime-character-controller
12. character-production-qa
13. character-production-orchestrator

Full [skill documentation and templates](plugins/unity-character-creation-plugin/README.md). The orchestrator initializes missing `character-production/CHARACTER_SPEC.md`, `DIRECTION_SPEC.md`, `ANIMATION_SPEC.md`, `EQUIPMENT_SPEC.md` and `QA_CHECKLIST.md` inside the target game repository, preserving existing specifications. The plugin cache/source is not the game project.

**Every skill invocation makes a fresh Unity plugin readiness/discovery call.** The official Unity plugin/tooling is separately installed; this plugin packages the workflows and templates. If the Unity bridge cannot connect, the skill reports the blocker and stops dependent production. [Unity setup](plugins/unity-character-creation-plugin/skills/character-production-orchestrator/references/unity-plugin.md).

## Package layout

```text
.agents/plugins/marketplace.json
plugins/unity-character-creation-plugin/
  .codex-plugin/plugin.json
  assets/icon.svg
  skills/                   # 13 skills, references and embedded templates
  character-production/    # project specification templates
  scripts/                 # optional standalone pack helpers
  README.md
scripts/
  open_plugin.py           # local View/Share links; optional app launch
  validate_plugin.py       # structure, references and manifest checks
docs/INSTALL.th.md
```

## Validate the plugin source

```bash
python scripts/validate_plugin.py
```

The plugin is registered through `.agents/plugins/marketplace.json`; its manifest discovers all 13 bundled skill directories. Installation happens in Codex Desktop after opening the generated View link. Remove an older standalone copy of these same 13 skills from a project's `.agents/skills/` after checking for custom edits if you choose plugin installation, to avoid duplicate discovery.

Manifest and marketplace structure follow OpenAI's [plugin examples](https://github.com/openai/plugins) and [plugin-creator specification](https://github.com/openai/plugins/blob/main/.agents/skills/plugin-creator/references/plugin-json-spec.md). Source/installer checks are separate from actual Desktop installation and Unity runtime verification.

## Gaps or missing parts during diagonal walking

Version 1.0.1 adds a [missing-parts diagnosis protocol](plugins/unity-character-creation-plugin/skills/character-production-orchestrator/references/missing-parts-diagnosis.md) and stronger evidence gates across parts, import, rigging, animation, direction switching, equipment and QA. For Southwest defects, isolate neutral, locked-direction walking and direction transitions before changing art or rigging. Preserve valid master artwork and inspect actual renderer/overlap/pivot/binding data.

```text
Use joint-skinning-validation from unity-character-creation-plugin.
Diagnose gaps during Southwest walking in my existing character.
Compare neutral, locked SW and South/West-to-SW transitions.
Inspect source overlaps, pivots, joint anchors, SpriteSkin bindings, resolver labels and sorting.
Record the confirmed cause and before/after rendered evidence. Do not regenerate my master character.
```

The updated workflows improve diagnosis and acceptance criteria; installing the plugin does not automatically repair existing artwork/prefabs. Existing canonical specs should gain the new evidence fields without being overwritten. Unity rendering and runtime tests remain project-level work.
