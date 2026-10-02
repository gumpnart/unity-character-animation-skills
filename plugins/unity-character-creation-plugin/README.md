# Unity 2.5D Character Plugin — skill workflows

A Codex plugin containing an Agent Skills pack for producing a modular 2.5D RPG character from its canonical master design through Unity 2D skeletal animation, equipment and runtime QA. This pack has **13 separate skills**; animation authoring accepts any named action with its own pose grammar.

ใช้สำหรับ workflow ตั้งแต่ Master Character → artwork แต่ละทิศ → แยกชิ้นส่วน → rig → animation → equipment → runtime โดยบันทึกข้อมูลลง specification กลาง ไม่พึ่งประวัติแชตเพียงอย่างเดียว เริ่มตรวจ South ก่อน แล้วขยายไปทิศและ action ที่ต้องการ

For Codex Desktop plugin installation, follow the [repository installation guide](../../docs/INSTALL.th.md). The optional standalone installer below copies skills into a game repository; it does not register or install a Codex plugin. Use one installation mode to avoid duplicate skill discovery.

## Skills

| Skill | Purpose |
| --- | --- |
| [master-character](skills/master-character/SKILL.md) | Identity, proportions, style and canonical base artwork |
| [direction-master](skills/direction-master/SKILL.md) | Neutral masters for all eight directions |
| [rig-ready-parts](skills/rig-ready-parts/SKILL.md) | 18 modular pieces, pivots and hidden overlap |
| [unity-asset-import](skills/unity-asset-import/SKILL.md) | PSD/PSB/PNG policy, PPU and import validation |
| [unity-skeleton-rig](skills/unity-skeleton-rig/SKILL.md) | Shared bones, prefab, sockets and SpriteSkin |
| [joint-skinning-validation](skills/joint-skinning-validation/SKILL.md) | Joint gaps, weights, pivots and pixel distortion |
| [animation-clip-authoring](skills/animation-clip-authoring/SKILL.md) | Arbitrary skeletal actions and phase grammar |
| [directional-animation-system](skills/directional-animation-system/SKILL.md) | 4/8 direction clips, artwork and depth coordination |
| [modular-equipment-system](skills/modular-equipment-system/SKILL.md) | Clothing, armor, weapon swaps and hand attachments |
| [animator-controller](skills/animator-controller/SKILL.md) | Locomotion, actions, interrupts and completion |
| [runtime-character-controller](skills/runtime-character-controller/SKILL.md) | Movement, facing, sorting and gameplay integration |
| [character-production-qa](skills/character-production-qa/SKILL.md) | Artwork-to-runtime checks and defect routing |
| [character-production-orchestrator](skills/character-production-orchestrator/SKILL.md) | Initialize specs, select stages and resume from disk |

## Optional standalone skill installation into a Unity repository

Install the official Unity plugin in Codex and connect your target Unity Editor first. **Every invocation of every skill makes a fresh actual Unity plugin readiness/discovery call**, including master design and reviews. The pack cannot embed or enable the official plugin; setup is documented in [Unity integration](skills/character-production-orchestrator/references/unity-plugin.md). If it cannot connect, the invoked skill reports the blocker and stops dependent production.

Clone this repository to a separate location, then run:

```bash
git clone https://github.com/gumpnart/unity-character-creation-plugin.git
python unity-character-creation-plugin/plugins/unity-character-creation-plugin/scripts/install_pack.py --project "/absolute/path/MyUnityGame"
```

The installer copies all 13 directories to `.agents/skills/` and initializes missing canonical specs under `character-production/`. Existing specs are preserved. If pack skill directories already exist, use `--replace-skills` deliberately to update those 13 directories. After inspecting a known legacy installation, `--remove-legacy` removes only `.agents/skills/unity-eight-direction-character`; it never deletes unrelated skills or assets. Open a new Codex session in the Unity project after installation.

```text
MyUnityGame/
├── .agents/skills/
│   ├── character-production-orchestrator/SKILL.md
│   ├── master-character/SKILL.md
│   ├── direction-master/SKILL.md
│   └── ... (13 separate skill directories)
├── character-production/
│   ├── CHARACTER_SPEC.md
│   ├── DIRECTION_SPEC.md
│   ├── ANIMATION_SPEC.md
│   ├── EQUIPMENT_SPEC.md
│   └── QA_CHECKLIST.md
└── Assets/
```

Manual installation: copy `skills/*` to `.agents/skills/`; copy each `character-production/*.template.md` to its corresponding name without `.template` in the Unity project, plus QA_CHECKLIST.md. Never replace populated canonical specs with blank templates. Templates are also bundled with the orchestrator so it can initialize missing specs after manual skill installation.

## Start or resume

```text
Use the $character-production-orchestrator skill.

We are starting a new modular 2.5D RPG character.
Start from the master-character stage.
Inspect the repository first.
Create the character production specification before proceeding
to directional artwork or skeletal rigging.
Do not skip stages.
```

Or invoke a particular stage:

```text
Use the $master-character skill.
Create the canonical base design and record measured proportions in CHARACTER_SPEC.md.
```

```text
Use the $animation-clip-authoring skill.
Create a sword light-attack animation for the South direction.
Use anticipation, acceleration, contact, follow-through and recovery.
Record timing, poses, markers and completion behavior in ANIMATION_SPEC.md.
```

```text
Use the $directional-animation-system skill.
Expand the validated SwordLightAttack to all eight directions.
Preserve anatomical limb identity and validate each direction's grip and sorting.
```

## Actions and durable specifications

Supported examples include Idle/Walk/Run/Sprint; Jump/Landing/Dodge/Roll; Attack/HeavyAttack/Combo/SkillAttack/BowAttack/TwoHandAttack; Cast/Channel/Release; Hit/Knockback/Death; Interact/UseItem/Emote. These are examples, not a hard-coded action enum. Melee uses anticipation → acceleration → contact → follow-through → recovery; casting uses anticipation → channel/hold → release → recovery. The five-key walk grammar applies only to the baseline walk.

Templates: [character](character-production/CHARACTER_SPEC.template.md), [directions](character-production/DIRECTION_SPEC.template.md), [animation](character-production/ANIMATION_SPEC.template.md), [equipment](character-production/EQUIPMENT_SPEC.template.md), [QA](character-production/QA_CHECKLIST.md).

Shared references: [production contract](skills/character-production-orchestrator/references/production-contract.md), [stage map](skills/character-production-orchestrator/references/stage-map.md), [action grammar](skills/character-production-orchestrator/references/action-grammar.md), [equipment contract](skills/character-production-orchestrator/references/equipment-contract.md).

Record actual measurements and source revisions after master validation; later stages must follow them. Upstream changes mark affected outputs stale. Existing validated assets can be reused after inspection. Scripts and docs are reusable workflows; the pack does not include finished character artwork or claim a Unity prefab has already been generated or tested.

## Pack verification

```bash
python scripts/validate_pack.py
```

This checks all 13 skill names/front matter, shared links, required templates and Unity invocation instructions. Unity compilation, rendered assets and Play mode results belong to the target project's production QA, not these pack checks.
