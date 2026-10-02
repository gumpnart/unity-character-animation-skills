# Equipment and Sprite Library contract

Use categories corresponding to the 18 body parts plus six attachments. The socket names are Weapon_R, Weapon_L, HeadEquipment, ChestEquipment, BackEquipment, WaistEquipment. Labels encode the selected appearance and direction; for example `Base_South`, `LeatherArmor_South`, `Sword_South`, `None_South`. Record one consistent naming convention in EQUIPMENT_SPEC.md.

| Item family | Typical renderer/resolver categories | Shared rig usage |
| --- | --- | --- |
| Shirt/armor | Torso_Upper, Torso_Lower, optional UpperArm/Forearm parts | same torso/arm bones; body appearance replacement or compatible overlays |
| Helmet | HeadEquipment | Head socket |
| Cape/back item | BackEquipment, additional overlay parts if needed | Chest socket/shared calibrated bones |
| Belt | WaistEquipment | Pelvis socket |
| Sword/axe/spear/staff/relic | Weapon_R or Weapon_L | real hand socket, per-direction grip |

One item can map several resolvers. Multiple pieces on one slot can be a socket prefab with child resolvers; there is no requirement that one category equals one entire item. Library assets must include the active item's labels for every supported direction. Do not replace a complete character library with a weapon-only library and accidentally remove body categories.

Minimal equipment data: item ID, slot ID, compatible rig revision, per-direction category→label mappings, grip/local pose offsets, required direction coverage, optional socket prefab. Runtime selection validates the complete item/direction data before applying changes. Empty slots use explicit empty sprites/labels and clear prior prefabs. Loading saved equipment must preserve item IDs and fail usefully for removed data.

For mesh-deformed clothing, sprite skin metadata and shared bone/mesh compatibility must be checked. A resolver selecting a new Sprite does not by itself make an incompatible SpriteSkin rig valid. Rigid pieces avoid many binding problems but still require matching pivots and scale.

Test changing BaseShirt→LeatherArmor→IronArmor while IdleSouth/WalkSouth continue playing, then Sword→Axe on Weapon_R. Verify unchanged skeleton/clip assets, stable pivots, hand grip and part sort order. Test supported requested directions and the action that uses the item. RareArmor/RelicWeapon are ordinary item data entries, not new animation architectures.
