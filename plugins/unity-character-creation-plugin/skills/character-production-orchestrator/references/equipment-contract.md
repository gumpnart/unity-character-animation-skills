# Equipment: shared source poses, layered runtime frames

Source slots remain Weapon_R→Hand_R, Weapon_L→Hand_L, HeadEquipment→Head, ChestEquipment/BackEquipment→Chest and WaistEquipment→Pelvis. Items equip on this authoring skeleton and reuse source AnimationClips; the rendered item frames encode the movement/grip for runtime. New items need actual baked coverage for their supported actions/directions.

Runtime categories describe passes (Body, Hair_Back/Front, Clothing_Upper/Lower, Armor_Back/Front, Shoes, Weapon_Back/Front, Accessories, with Body/Outfit front passes when required). Labels can be `Appearance_Action_Direction_fNNN`; record the actual convention and compatible bank/sample revisions. Every pass uses one authoritative action/direction/index. An equipment library must not discard body mappings.

Minimal item data: item/slot ID, source art and authoring attachment/grip, supported action/direction banks, sample schedule compatibility, resolver/pass mappings, replaced categories, overlays/masks, front/back occlusion and optional per-frame FX/grip anchors. A static socket offset alone cannot produce a complete animated baked weapon bank.

Base master outfit is a fitted sleeveless top and shorts with bare feet. Torso armor can replace Clothing_Upper while retaining Clothing_Lower. Boots cover the appropriate foot regions with preserved ground anchor. Hair/headgear uses recorded hair visibility/masks. An overlay must hide covered base pixels through matching masks/pass precompositing, or explicitly replace the appropriate category; do not let the base shirt/feet leak through.

One flat body and one flat item cannot support every hand/torso overlap. Use split front/back body/item passes or tested masking, then compare the composite to the fully equipped source pose. Baked image cleanup that changes a contour must update affected masks/item occlusion.

At swap time validate the full required frame set and coverage before applying. Retain the last complete valid appearance on failure; optional empty slots are explicit. Keep the same current pose index, action/time and anatomical identities. Test BaseClothing→LeatherArmor→IronArmor and Sword→Axe during idle/walk/action and SW when required, without recreating shared authoring clip timing.
