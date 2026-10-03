# Shared character production contract

## Persistent state

Find the target Unity project root, normally containing `Assets/`, `Packages/` and `ProjectSettings/`. This skill pack is not itself a Unity project. All stages read `character-production/` under that project. Use one active character per directory initially; for multiple characters use `character-production/characters/<character-id>/` and record the active ID/path in the top-level CHARACTER_SPEC.md. Do not mix measurements between characters.

| File | Owns |
| --- | --- |
| CHARACTER_SPEC.md | Identity, approved master, canonical measurements, art/import policy, shared rig, stage ledger |
| DIRECTION_SPEC.md | Neutral masters, part inventories, calibrated rest poses, pivots, sort orders, sockets, direction coverage |
| ANIMATION_SPEC.md | Arbitrary action IDs, phase grammar, timing/pose tables, markers, clip/direction coverage, controller/runtime contract |
| EQUIPMENT_SPEC.md | Slots, items, resolver labels, compatible rig revisions, grip offsets, equipment coverage |
| QA_CHECKLIST.md | Evidence, issues, checks and scoped release results |

Revision fields and evidence paths are required. Statuses: `pending` (not done), `draft` (proposed), `ready` (produced, awaits checks), `validated` (checks passed with evidence), `blocked` (missing prerequisite), `stale` (upstream changed). Planned assets, mock responses and compilation-free inspection cannot establish runtime validation. Record the actual mode used: static, rendered preview, Edit mode or Play mode.

Update only the fields owned by the completed work and its dependencies. Keep existing canonical values unless revising them intentionally. Log upstream revision changes and mark affected outputs stale. An explicit user instruction overrides a prior spec; reconcile the spec before doing dependent work. Aesthetic approval gates apply only when requested; report unresolved design decisions honestly without inventing approval. No mandatory repeated permission prompts for routine reversible work.

## Camera and physical identity

2.5D means projected 2D artwork in an RPG world with a slightly elevated camera. South moves toward the bottom of the screen and generally faces the camera. Record actual world/screen axes for the project. Character L/R always means the character's anatomical side; screen-left changes with direction. Never swap bone identity or mirror a character to create the opposite walk contact. All animations belong to the same skeleton, including equipment variants.

Eight canonical directions: South, SouthWest, West, NorthWest, North, NorthEast, East, SouthEast. Short names SW/NW/NE/SE are presentation aliases, not extra IDs. New production validates South first; requested scope can be all eight. Do not require new art when a suitable existing validated character already supplies it.

## Exact source-part names

```text
Head: Hair_Back, Head, Hair_Front
Body: Torso_Upper, Torso_Lower, Pelvis
Arm_L: UpperArm_L, Forearm_L, Hand_L
Arm_R: UpperArm_R, Forearm_R, Hand_R
Leg_L: Thigh_L, Shin_L, Foot_L
Leg_R: Thigh_R, Shin_R, Foot_R
```

The Head/Body/Arm/Leg entries above are organizational layer groups, not additional skeleton bones. Each joint has hidden artwork overlap; resolve rotation gaps in source art before adding heavy deformation.

## Shared bone hierarchy

```text
Root
└── Pelvis
    ├── SpineLower
    │   └── SpineUpper
    │       └── Chest
    │           ├── Neck
    │           │   └── Head
    │           ├── Shoulder_L
    │           │   └── UpperArm_L
    │           │       └── Forearm_L
    │           │           └── Hand_L
    │           └── Shoulder_R
    │               └── UpperArm_R
    │                   └── Forearm_R
    │                       └── Hand_R
    ├── Thigh_L
    │   └── Shin_L
    │       └── Foot_L
    └── Thigh_R
        └── Shin_R
            └── Foot_R
```

Twenty-one bones including Root. Six equipment sockets are additional attachments, not independent bones/rigs: Weapon_R→Hand_R, Weapon_L→Hand_L, HeadEquipment→Head, ChestEquipment→Chest, BackEquipment→Chest, WaistEquipment→Pelvis.

## Suggested prefab and Unity project layout

```text
Character (movement/input/action owner, Animator, SpriteLibrary, SortingGroup)
├── Collision / gameplay components
├── VisualRoot (local bob only)
│   └── Root (shared bones above; renderers beneath matching bones)
│       └── ... Hand_R / Weapon_R, Hand_L / Weapon_L, etc.
└── Shadow (ground-plane visual if needed)

Assets/Characters/<CharacterId>/
  Source/              # layered artwork / exported pieces
  Sprites/             # imported sprites and directional libraries
  Prefabs/             # character and optional equipment prefabs
  Animation/Clips/     # Action_Direction clips
  Animation/Controllers/
  Equipment/           # item data
Assets/Scripts/Characters/Runtime/
Assets/Scripts/Characters/Editor/
Assets/Scenes/CharacterTest.unity
character-production/  # persistent specs and evidence
```

Adapt to existing repository conventions. Animate correct relative binding paths; actual imported hierarchy takes precedence over a hypothetical path. Keep locomotion displacement off the visual skeleton unless explicitly using root motion.

## Rendering and import rules

Use Unity 2D Animation, PSD Importer, SpriteLibrary, SpriteResolver and Animator where appropriate. Verify package/editor compatibility instead of hard-coding package versions. The current official CLI/Pipeline bridge requires Unity 6.0+; do not upgrade an existing project silently. Load relevant official Unity package/sprite/pixel-perfect skills when needed.

Prefer rigid limb sprite pieces and bone transforms; use minimal joint weights where necessary. Point filtering and consistent PPU are the default for pixel art. Record pivots, full-canvas/crop offsets, pixel grid behavior and native-scale preview evidence. Bones can move subpixel in world units, so check rendered contours rather than claiming Point filtering alone guarantees perfect pixels.

Use the live Editor for scenes, prefabs, libraries, clips and serialized data. Never create guessed GUID/fileID YAML. Source PSD/PSB editing needs an actual supported art tool; report missing tools rather than inventing editable layered assets. The mandatory plugin instructions are workflow requirements, not a plugin dependency manifest and not runtime gameplay calls.

## Rendered moving-silhouette gate

Every claimed validated action/direction must satisfy the [missing-parts diagnosis evidence gate](missing-parts-diagnosis.md): actual required-part inventory, per-direction attachment/overlap/visibility contract, phase and intermediate/extreme pose sampling, rendered continuous playback and relevant direction/equipment transitions. A structurally valid rig or a single neutral image is insufficient. Keep suspected causes separate from confirmed causes and plugin checks separate from a repaired Unity character.
