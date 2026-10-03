# Unity Character Creation — Codex Desktop Plugin

**`unity-character-creation-plugin` · v2.0.0 · 13 skills**

Create modular 2.5D pixel RPG characters through a hybrid production pipeline:

```text
Neutral master → direction masters → modular source parts → shared authoring rig
→ arbitrary action clips → bake registered layers → pixel cleanup
→ synchronized layered frame playback → equipment swaps → visual QA
```

เวอร์ชันนี้ใช้ rig ช่วยสร้าง pose และ animation แล้ว bake เป็นเฟรมสำหรับเกม แยก Body / Hair / Clothing / Armor / Weapon ตาม render passes ที่ตรวจสอบแล้ว ใช้เวลาและ frame index ร่วมกันทุกชั้น รองรับ 8 ทิศและ action ที่ตั้งชื่อเองได้ โดยเริ่มพิสูจน์ South ก่อนขยายงาน

Master เริ่มจากท่ายืนกลาง ใส่เสื้อแขนกุดพอดีตัว กางเกงขาสั้น และเท้าเปล่า ไม่มีเกราะหรืออาวุธ หากมี master ที่ยอมรับแล้ว ให้รักษา identity และ artwork เดิมไว้

## Install in Codex Desktop

Clone this source repository into a permanent local folder on the computer running Codex Desktop:

```bash
git clone https://github.com/gumpnart/unity-character-creation-plugin.git
cd unity-character-creation-plugin
python scripts/open_plugin.py
```

Open the printed **View plugin** `codex://plugins/...` link on that computer, or run `python scripts/open_plugin.py --open`. Choose **Install** and enable the plugin in Codex Desktop. The helper uses your actual local marketplace path. A cloud workspace path cannot locate files on your desktop.

Install/connect the official **Unity** plugin separately and open the target Unity project. This package contains workflow instructions, references and templates; it does not bundle the Unity bridge, a finished exporter or game assets. Each production skill must discover and call the real Unity tools on **every invocation**. If the bridge/project cannot connect, dependent production stops with the blocker recorded.

See [installation and update instructions in Thai](docs/INSTALL.th.md) and [Unity integration requirements](plugins/unity-character-creation-plugin/skills/character-production-orchestrator/references/unity-plugin.md).

## Start or resume production

```text
Use character-production-orchestrator from unity-character-creation-plugin.
Inspect my Unity project first.
Use hybrid-baked-frames: rig-assisted authoring, baking, pixel cleanup, layered runtime frames.
Start from master-character; create the six canonical specifications.
Prove South Idle/Walk with one outfit and one weapon before expanding all eight directions.
Do not skip visual validation.
```

For an existing rigged character:

```text
Use character-production-orchestrator from unity-character-creation-plugin.
Migrate my existing character to the v2 hybrid pipeline.
Preserve its accepted master artwork, source rig, clips and item IDs.
Add missing specification fields, bake and clean South Idle/Walk,
then validate synchronized runtime layers and equipment swapping.
```

For any new action:

```text
Use animation-clip-authoring from unity-character-creation-plugin.
Create SwordLightAttack for South.
Use anticipation → acceleration → contact → follow-through → recovery.
Author the source clip, bake all required layers, clean pixels and validate final playback.
```

Select the skill contributed by this plugin if Codex shows qualified names. Custom mining, fishing, climbing, dance and other actions are supported. Walk's pose grammar is not imposed on other actions.

## Included skills and durable specifications

All 13 stage names remain available: `master-character`, `direction-master`, `rig-ready-parts`, `unity-asset-import`, `unity-skeleton-rig`, `joint-skinning-validation`, `animation-clip-authoring`, `directional-animation-system`, `modular-equipment-system`, `animator-controller`, `runtime-character-controller`, `character-production-qa`, `character-production-orchestrator`.

The orchestrator initializes missing files in the **target game repository**, preserving existing values:

```text
character-production/
  CHARACTER_SPEC.md
  DIRECTION_SPEC.md
  ANIMATION_SPEC.md
  EQUIPMENT_SPEC.md
  FRAME_BANK_SPEC.md
  QA_CHECKLIST.md
```

`FRAME_BANK_SPEC.md` records real canvas, ground pivot, exact sample times, frame count, render passes, appearance coverage, labels, markers and optional anchors. All layers must use the same source pose and timing. Equipment reuses source animations but new items still require actual baked/cleaned frames for their requested coverage.

See the [complete workflow guide](plugins/unity-character-creation-plugin/README.md), [baking and cleanup contract](plugins/unity-character-creation-plugin/skills/character-production-orchestrator/references/bake-and-cleanup.md), and [layered playback design](plugins/unity-character-creation-plugin/skills/character-production-orchestrator/references/layered-frame-playback.md).

## Updating from v1

**v2.0.0 changes the default gameplay visual pipeline.** Source bones/clips remain authoring assets; gameplay uses cleaned frame banks rather than live deformation of modular pieces. Installing the plugin does not automatically bake frames, repair artwork or convert a runtime prefab. Follow the [migration guide](plugins/unity-character-creation-plugin/skills/character-production-orchestrator/references/migration-v2.md) in the game project. Add new fields to populated specs rather than replacing them.

Keep the plugin identifier `unity-character-creation-plugin`. For an old clone using the previous repository URL:

```bash
git remote set-url origin https://github.com/gumpnart/unity-character-creation-plugin.git
```

If standalone copies of these skills exist in your game's `.agents/skills/`, check for custom edits and remove duplicates when switching to plugin discovery.

## Diagnose missing parts

Compare the **same pose/time** in the source rig, raw bake, cleaned PNG and runtime composite. This isolates overlap/rig problems from export masks, cleanup errors, bad pivots or incorrect layer selection. For Southwest, inspect neutral SW, fixed SW walking and direction switches separately. Preserve valid master art.

```text
Use character-production-qa from unity-character-creation-plugin.
Diagnose gaps during Southwest walking.
Compare source pose, raw bake, final frame and runtime composite at the same time/index.
Record the confirmed cause and before/after rendered evidence.
```

See the [diagnosis protocol](plugins/unity-character-creation-plugin/skills/character-production-orchestrator/references/missing-parts-diagnosis.md). The hybrid workflow improves control of pixel silhouettes; each final frame still needs review.

## Package structure and validation

```text
.agents/plugins/marketplace.json
plugins/unity-character-creation-plugin/
  .codex-plugin/plugin.json
  assets/icon.svg
  skills/                   # 13 workflows, references, mirrored templates
  character-production/    # six specification templates
  scripts/                 # optional standalone installation and validation
scripts/open_plugin.py
scripts/validate_plugin.py
docs/INSTALL.th.md
```

```bash
python scripts/validate_plugin.py
```

This is installable plugin **source**, not a ZIP uploaded to GitHub. The marketplace and manifest follow OpenAI's [plugin examples](https://github.com/openai/plugins) and [plugin schema](https://github.com/openai/plugins/blob/main/.agents/skills/plugin-creator/references/plugin-json-spec.md). Source validation is separate from actual Desktop installation, Unity export and game runtime testing.
