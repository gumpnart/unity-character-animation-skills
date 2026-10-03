# One clock for layered frame playback

The default runtime has no animated limb bones or SpriteSkin deformation. A root action/state owner (typically the existing/root Animator plus one frame driver) supplies action, direction, time and equipment. The frame driver owns sprite selection for every body/hair/outfit/armor/weapon pass.

## Minimal data

Action bank: arbitrary action ID, direction, duration, loop/one-shot/hold, sample timestamps or fixed FPS/frame count, timeline markers, pass layout and revision. Appearance/item bank: item ID, supported action/direction keys, per-pass sprite/resolver selections indexed by the SAME pose index, replacement/overlay/mask rules and optional per-frame anchors. Store these as ScriptableObjects or existing project data; do not create a universal framework for the first slice.

For SpriteLibrary/Resolver, categories represent render passes; labels can encode `Appearance_Action_Direction_fNNN`. The shared driver resolves every pass for one index. Item libraries must cover active body/item selections; replacing the whole library with a weapon-only library is unsafe. A direct sprite array asset is also useful for storage, but the requested equipment interface still selects/validates appearance mappings through the library/resolver contract.

## Update algorithm

1. Read the single authoritative action clock. Normalize/wrap only loops; clamp/hold one-shots according to their state contract. Compute one index from the recorded schedule. For fixed FPS select floor(time*FPS) within the bank's valid count; handle floating-point end boundaries deliberately. Variable schedules select the last sample timestamp not exceeding time.
2. Resolve body and the entire loadout for that action/direction/index into a pending set. Check required data, canvas/pivot/PPU/schedule compatibility, replacement masks and pass order. Optional empty slots may be transparent. On a required missing mapping, retain the last complete valid view and report the key; do not clear one limb/armor layer.
3. Apply all sprite/resolver selections, mask/visibility and internal ordering before the next rendered frame. No per-layer clock/Animator, no independent weapon frame counter, no frame interpolation or per-limb rotation. Full-canvas sprites share the ground anchor. Record intentional whole-character flips only for explicitly validated symmetric art; default directions are independent banks.
4. Preserve normalized gait phase when changing direction. All layers select the target direction's matching pose on the same update; if counts differ, map via shared normalized/sample time explicitly. Prefer equal schedules for compatible variants. An action-facing lock keeps the specified direction until allowed to turn.
5. Equipment swaps select the same current pose index after validating full coverage; do not reset body or item time. Apply replacement versus overlay rules atomically. Clothing_Upper replacement does not implicitly remove Clothing_Lower. Per-frame anchors support gameplay FX/projectiles; weapon placement itself is normally baked from source sockets.
6. Dispatch semantic markers crossed since the previous update, handling wrap, multiple loops/large time steps, speed changes and action entry/exit. Frame equality checks can drop markers at low frame rate. Gameplay owns hits/items/physics and consumes signals once; playback does not independently apply effects.

## Example runtime hierarchy

```text
Character (world movement, input/action owner, Animator, SortingGroup)
├── Collision / gameplay
├── VisualRoot (shared ground anchor)
│   ├── Body/Body_Back/Body_Front pass renderers as required
│   ├── Hair_Back / Hair_Front
│   ├── Clothing_Upper / Clothing_Lower / Shoes
│   ├── Armor_Back / Armor_Front
│   ├── Weapon_Back / Weapon_Front
│   └── Accessories (each pass has SpriteRenderer + SpriteResolver)
└── Shadow (ground plane)
```

Only instantiate the passes actually required by the approved composition. Sorting order is per direction/action/frame where needed; flat layers cannot represent arbitrary interleaving without split passes or masks. SortingGroup ground order follows the feet/world position and remains distinct from internal ordering or baked body bob.

Acceptance tests: fixed South and SW loops; idle↔walk; direction changes at contact/passing; armor/weapon swap mid-cycle; one-shot return/terminal hold; facing locks/interrupts; low frame rate and changed speed; missing-item frame; several characters/props. Assert all required renderers use one action/direction/pose index and the composite preserves the approved silhouette. Test real rendered gameplay, not just array lengths.
