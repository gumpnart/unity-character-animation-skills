# Equipment specification

Character ID / rig revision / direction spec revision: TBD
Revision / status: 0 / draft
SpriteLibrary asset paths / resolver convention / empty labels: TBD
Swap owner / validation and missing-label behavior: TBD

## Slots

| Slot | Shared bone socket parent | Expected content |
| --- | --- | --- |
| Weapon_R | Hand_R | Sword / Axe / Spear / Staff / RelicWeapon |
| Weapon_L | Hand_L | Offhand / two-handed grip support |
| HeadEquipment | Head | Helmet / head accessory |
| ChestEquipment | Chest | Shirt / LeatherArmor / IronArmor / RareArmor |
| BackEquipment | Chest | Cape / back accessory |
| WaistEquipment | Pelvis | Belt / waist accessory |

## Item record (repeat per item)

Item ID / slot / data asset: TBD
Source paths / item revision / compatible rig: TBD
Required directions / current coverage: TBD
Replaced body categories / overlay renderers / socket prefab: TBD
Shared SpriteSkin compatibility or rigid-piece policy: TBD
Two-hand grip / pose requirements: TBD

| Direction | Resolver category → label mappings | Socket position / rotation / scale | Part draw order | Status / evidence |
| --- | --- | --- | --- | --- |
| TBD | TBD | TBD | TBD | pending |

Swap/clear validation, save/load IDs, retained animation state: TBD
IdleSouth / WalkSouth / action grip / direction-swap test evidence: TBD
Revision history and affected stale outputs: TBD

## v2 equipment bank and replacement record

The socket table above is for shared source rigs during authoring/baking. Runtime equipment uses matching frame banks; optional live VFX use validated per-frame anchors. Item sprites do not automatically follow a gameplay skeleton.

Item ID / appearance revision / compatible action-bank and layout revisions: TBD
Source rig socket / authoring grip offsets / baked anchor track: TBD
Mode: replacement / overlay / masked overlay (choose per pass)
Independent clothing regions: upper garment / lower garment / footwear / head / back / waist
Exact hidden/replaced regions / mask assets / uncovered required body regions: TBD
Render passes / split near-far geometry / empty transparent labels: TBD
Runtime SpriteLibrary / category-label convention / item data asset: TBD

| Action | Direction | Required frame count / timing revision | Required pass banks / valid labels | Missing frames / acceptance evidence |
| --- | --- | --- | --- | --- |
| TBD | TBD | TBD | TBD | pending |

Changing a shirt must not remove shorts. Ensure a replacement mask covers the old garment without hiding unrelated anatomy. All item/pass sprites share the approved full canvas, anchor, sample schedule and source pose revision. New items generally require baking and cleanup across requested actions/directions; source clip reuse does not supply missing item frames.
Swap/clear policy: prevalidate every required layer and apply together at the current frame; retain the previous complete outfit on failure. Record save/load stable item IDs, empty-slot sprites, phase preservation and weapon-hand alignment evidence.
