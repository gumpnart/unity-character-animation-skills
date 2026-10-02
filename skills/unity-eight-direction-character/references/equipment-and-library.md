# Directional library and equipment

Use one shared library with fixed per-piece categories. The direction appears in
labels, for example `BaseSouth`, `BaseNorthWest`, `LeatherArmorEast`, `SwordNorth`.
Do not exchange UpperArm_L and UpperArm_R categories when facing away.

## Categories

- Head: Hair_Back, Head, Hair_Front.
- Body: Torso_Upper, Torso_Lower, Pelvis.
- Arms: UpperArm_L, Forearm_L, Hand_L; UpperArm_R, Forearm_R, Hand_R.
- Legs: Thigh_L, Shin_L, Foot_L; Thigh_R, Shin_R, Foot_R.
- Equipment: Weapon_R, Weapon_L, HeadEquipment, ChestEquipment, BackEquipment,
  WaistEquipment.

For each direction suffix, create Base<Direction> for ordinary body pieces,
BaseShirt<Direction> for both torso parts, and None<Direction> for empty equipment
sockets. None is a real transparent sprite with the correct pivot/PPU, not a null
reference. Profiles list every bound category once, including equipment defaults.

Initial test items are LeatherArmor and IronArmor (both torso pieces), Sword_R and
Axe_R (Weapon_R). Add other weapons or armor after the eight-view test items work.

## Data

`DirectionalCharacterProfile` contains eight unique DirectionPose entries:

- Complete default category/label choices for all resolver bindings.
- Complete fixed renderer orders for those same categories.
- Captured bone rest positions for the exact shared hierarchy.
- Idle and four walk limb-depth poses.
- Stride, bob, arm amplitude, knee bends, and foot-lift authoring parameters.
- Empty/default weapon socket local poses.

`DirectionalEquipmentDefinition` contains one stable itemId and slot, plus eight
DirectionItemAppearance variants. Each variant provides exactly one choice for
every equipment-controlled category in its slot, and a full weapon socket local
pose when applicable. Socket pose is relative to Hand_L/Hand_R, not world space.

The resolver binding marks whether a category is equipment-controlled and, if so,
which slot owns it. Base head/hair/limb parts can remain uncontrolled. Torso_Upper
and Torso_Lower belong to Chest. ChestEquipment can remain an uncontrolled empty
overlay when the item swaps torso sprites; bind it to Chest only if every chest
item also supplies its overlay label.

## Swap and missing variants

The template prevalidates the complete proposed appearance before changing labels,
equipment ownership, active facing, or sockets. The item API requires eight unique
variants; actual sprites and grip calibration are checked when applying a direction.
Before shipping, validate all eight directions with each test item equipped.

Do not silently fall back to a SOUTH weapon during a north-facing turn. Report
the missing item/direction/category and retain the previous appearance. Unequip
uses that direction's default labels and weapon socket poses. Changing equipment
does not restart the clip or move the physical bones.

## Weapons

Weapon_R socket stays beneath Hand_R; Weapon_L stays beneath Hand_L. Their
resolvers must be at or below the corresponding socket and within the same shared
SpriteLibrary. Hand grouping supplies world limb depth; the weapon renderer's
order relative to that hand is set by the direction profile.

Author blade/grip pivots separately for each view. Use per-direction positive
socket scale and position/Z rotation to calibrate the grip. Side or back weapons
may be occluded by the torso, but must still follow their physical hand. Back and
waist equipment attach to Chest and Pelvis, never to a separate Animator.
