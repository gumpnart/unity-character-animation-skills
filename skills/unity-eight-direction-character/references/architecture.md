# Shared eight-direction architecture

## Unity project layout

```text
Assets/Game/Characters/
  Art/Source/{South,SouthWest,West,NorthWest,North,NorthEast,East,SouthEast}/
  Art/Imported/                   # PSD Importer output
  Libraries/HeroEightDirection.spriteLib
  Profiles/HeroDirections.asset
  Prefabs/Character8.prefab
  Animations/EightDirection/      # 8 idle clips, 8 walk clips, controller
  Equipment/                     # directional item ScriptableObjects
  Scripts/Runtime/
  Scripts/Editor/
  Scenes/EightDirectionTest.unity
```

Use the same source-layer naming in each view: Hair_Back, Head, Hair_Front;
Torso_Upper, Torso_Lower, Pelvis; UpperArm_L, Forearm_L, Hand_L; UpperArm_R,
Forearm_R, Hand_R; Thigh_L, Shin_L, Foot_L; Thigh_R, Shin_R, Foot_R. These are
18 source pieces per view. Anatomical L/R always names the same physical limb.

## Prefab

```text
Character8                         # Animator + EightDirectionCharacter
  Visual                           # SpriteLibrary + SortingGroup
    Root                           # shared bone hierarchy below
      Pelvis
        ...
  GroundShadow                     # optional, outside animated pelvis bob
```

Animator paths start `Visual/Root/...`. World movement applies to Character8;
gait and direction-rest motion apply to the descendant bones. The SpriteLibrary
on Visual serves all resolvers. The top SortingGroup keeps the character together
relative to other characters; it is separate from internal limb sorting.

## Exact bone hierarchy

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

This is 21 bones including Root. Put anatomical joints at their proper artwork
positions. Directional rest positions belong to directional animation profiles;
they do not require a second skeleton.

Keep the Root origin and ground/foot anchor consistent across views so turns do
not jump the character in world space. Sprite-child local transforms remain fixed
across directions: use compatible sprite pivots and captured bone positions to
align artwork. The starter profile does not store separate renderer-local offsets.

## Rigid parts and sockets

Each rigid sprite is a child of its bone with SpriteRenderer + SpriteResolver.
Keep sprite children separate from bones; the clips key bones, not appearances.

| Bone | Sprite children / socket children |
|---|---|
| Pelvis | Pelvis_Visual, WaistEquipment |
| SpineLower | Torso_Lower |
| Chest | Torso_Upper, ChestEquipment, BackEquipment |
| Head | Hair_Back, Head_Visual, Hair_Front, HeadEquipment |
| UpperArm_L / UpperArm_R | corresponding upper-arm sprite |
| Forearm_L / Forearm_R | corresponding forearm sprite |
| Hand_L / Hand_R | corresponding hand sprite, Weapon_L / Weapon_R |
| Thigh_L / Thigh_R | corresponding thigh sprite |
| Shin_L / Shin_R | corresponding shin sprite |
| Foot_L / Foot_R | corresponding foot sprite |

A socket has its own SpriteRenderer + SpriteResolver or a fixed Appearance child.
Weapon sockets are direct children of their hands. Each view's weapon grip pivot
matches the hand origin. Item data sets socket-local pose; Animator never keys it.

Add nested SortingGroups directly on UpperArm_L, UpperArm_R, Thigh_L, and Thigh_R.
Their children, including weapons, follow their limb order. Use one Sorting Layer;
keep groups nested inside Visual (do not enable sorting at the root). Renderer
orders inside a limb group describe part overlap, while group orders describe
that limb's relationship to torso/other limbs. Both are direction-aware.

Chest items initially swap Torso_Upper/Torso_Lower. Keep ChestEquipment empty and
uncontrolled unless choosing an overlay design. Every slot-controlled piece must
be supplied by every item variant in that slot. Six sockets remain available:
Weapon_R, Weapon_L, HeadEquipment, ChestEquipment, BackEquipment, WaistEquipment.

## Pixel-art rigging

Use consistent PPU, Point filtering, no lossy texture compression, and no mipmaps.
Preserve crisp outlines. Avoid heavy deformation. Hide overlap at shoulder,
elbow, wrist, hip, knee, and ankle in every view and pose. Resolve gaps in source
art, not by stretching the mesh.

The starter builder supports upright bone axes, identity rest rotations, and unit
bone scales; orient the sprite children instead. Directional joint positions may
still differ. For an existing nonupright rig, adapt curves/bind axes or author
clips manually instead of rebuilding a working skeleton just to use this tool.

If using SpriteSkin, replacements require compatible mesh, weights, bone map,
and bind poses. SpriteResolver alone cannot reconcile incompatible skinning.
Do not apply a bone transform once through parenting and again through skinning.
Keep weapons rigid and attached to the existing physical hand.

## Migration from SOUTH

Retain bones, shared library categories, and source art. Add the other seven sets,
new profile/item assets, and the new controller. Remove SouthLocomotion,
SouthDepthOrder, and CharacterEquipment from the eight-direction prefab, then
wire EightDirectionCharacter. The templates use the RpgEightDirection namespace
so source files can coexist without duplicate class/enum errors.
