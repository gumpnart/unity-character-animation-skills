# Character production contract

Read [hybrid pipeline](hybrid-pipeline.md), [master design](master-design.md), [bake and cleanup](bake-and-cleanup.md) and [layered playback](layered-frame-playback.md). Default mode is `hybrid-baked-frames`; frame production replaces the old blanket frame-animation prohibition for this requested v2 workflow.

Canonical state lives under the actual Unity game project's `character-production/`: CHARACTER_SPEC.md, DIRECTION_SPEC.md, ANIMATION_SPEC.md, EQUIPMENT_SPEC.md, FRAME_BANK_SPEC.md, QA_CHECKLIST.md. For multiple characters use per-character subdirectories and record the active directory. Never mix identities or overwrite populated specs. Chat changes become recorded revisions; missing measurements stay TBD.

Statuses: pending, draft, ready, validated, blocked, stale. Validated requires actual evidence at the relevant stage: source artwork/rig, exported cleaned frames and runtime playback are separate gates. Upstream changes invalidate affected downstream artifacts. Explicit user requests override prior policy; record that reconciliation before production. Approval gates apply only when explicitly requested, not every reversible step.

South faces toward the camera/bottom of screen in a slightly elevated RPG view. L/R remains anatomical. Eight names: South, SouthWest, West, NorthWest, North, NorthEast, East, SouthEast. New production proves South first, then completes requested coverage. Directional skeletal poses are authoring assets; final gameplay selects their baked pixel frame banks.

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


## Authoring and runtime layout

The hierarchy above and sockets are source production assets. AnimationClip curves drive the authoring rig; clothing and weapons use it during bake. The default runtime uses the pass-renderer hierarchy in the layered playback guide, one action clock, SpriteLibrary/Resolver and ground SortingGroup. It does not need an independently deforming skeleton per item or live rotation of each limb.

Suggested assets under `Assets/Characters/<CharacterId>/`: Source/, Authoring/Prefabs/, Authoring/Clips/, Authoring/BakeScenes/, Baked/Raw/, Baked/Final/, Runtime/FrameBanks/, Runtime/Prefabs/, Equipment/. Respect existing conventions and preserve editable sources/cleanup separately.

Use Unity 2D Animation/PSD Importer for source production; verify actual package/editor compatibility. Use a real art tool for layered sources and pixel cleanup. The official CLI bridge supports Unity 6.0+; never silently upgrade an existing game. Every skill invocation requires a fresh actual Unity plugin call and connected target; use Editor APIs rather than guessed GUID/fileID YAML.

Point filtering, common PPU/canvas/ground anchor, correct alpha and actual native-scale inspection are required for final frames. Rig overlap and minimal weighting matter in production; per-frame cleanup plus coherent runtime layering preserve approved silhouettes. Bake files and runtime validation are not implied by plugin installation.

Every requested validated action/direction/loadout has final-frame inventory, masks/pass order, sample schedule, native renders and continuous gameplay evidence. Use [missing-parts diagnosis](missing-parts-diagnosis.md) to distinguish source gaps, bake/cleanup defects and runtime selection problems. Zero unexplained missing required regions; documented intentional occlusion is allowed.
