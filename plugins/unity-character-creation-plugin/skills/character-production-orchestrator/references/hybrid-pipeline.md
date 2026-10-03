# Hybrid pixel character production

Default mode: `hybrid-baked-frames`. Source rig → authoring action clips → deterministic layer baking → per-frame pixel review/cleanup → imported frame banks → synchronized runtime layer selection.

This v2 workflow replaces the earlier blanket ban on frame animation. Bones remain useful for authoring reusable actions and equipping source items; the runtime preserves approved pixel poses by selecting final images instead of deforming or rotating each limb. It reduces live joint-gap risk but still needs coherent source art, real exports, cleanup and equipment coverage. Bake is not an automatic guarantee of good pixel silhouettes.

## Persistent contracts

| File | Responsibility |
| --- | --- |
| CHARACTER_SPEC.md | Identity, neutral base outfit, proportions, pipeline mode, native scale, ground anchor, ledger |
| DIRECTION_SPEC.md | Direction source art, calibration, projection/occlusion and pass ordering |
| ANIMATION_SPEC.md | Any action ID, grammar, source clips, timing, sample schedule, markers and state ownership |
| EQUIPMENT_SPEC.md | Source items/sockets, runtime banks, replacements/overlays/masks and coverage |
| FRAME_BANK_SPEC.md | Raw/final frame paths, dimensions, pivot/PPU, schedule, pass layout, anchors and revisions |
| QA_CHECKLIST.md | Source, final-frame and gameplay evidence plus incident routing |

Never silently overwrite existing canonical specs. Record the accepted new pipeline, preserve valid identity/source art, and mark only affected exports/runtime checks stale. Keep all eight names and all arbitrary action IDs; new production validates South before broad expansion.

## Two distinct asset families

Authoring: 18 modular source body/hair pieces, clothing/equipment source layers, 21-bone shared rig, six sockets, per-direction calibration and skeletal AnimationClips. Source parts remain editable and use hidden overlap/minimal deformation.

Runtime: a small set of baked full-canvas render passes, for example Body, Hair_Back/Front, Clothing_Upper/Lower, Armor_Back/Front, Shoes, Weapon_Back/Front and Accessories. These are logical examples, not a fixed number of renderers. Flatten source pieces where safe, and split Body/Outfit front/back passes when a sword or sleeve must interleave with arms/torso. A single flat Body plus one flat Weapon cannot represent every overlap order.

Animations share a pose schedule across appearance variants. New equipment normally reuses source clips but requires baked item frames for the actions/directions it supports. This is the asset cost of crisp modular frame animation; do not claim an unbaked item works with every action.

## Common visual frame

Record one native canvas, RGBA export policy, ground anchor/pivot, PPU, orthographic bake camera and render-pass convention. Full-canvas frames are the initial default. If trimming/atlasing is introduced, restored offsets/pivots must produce the same composite alignment. No independently resized equipment frames or unrecorded crop offsets.

All required layers of an action/direction share exact duration/sample timestamps and one runtime clock. A fixed-FPS loop excludes its duplicated endpoint; a one-shot records its last-frame hold/return rule. Phase markers belong to the action timeline and remain distinct from image sampling.

## Clothing and source identity

The neutral master has a simple fitted sleeveless top, fitted shorts, bare feet and a defined baseline hairstyle by default. These are proposed baseline design choices, not immutable measurements. Body/face/proportions are canonical; hair and clothing can be replaceable appearances. No armor/weapon/cape obscures initial joint and ground landmarks.

Items declare replacement versus overlay. A chest armor can replace Clothing_Upper while retaining shorts; boots cover feet/Shoes without altering ground anchor. Overlays need explicit occlusion masks or precomposited passes when base clothing would otherwise leak. Keep covered source patches complete where supported outfits/motion expose them; do not require a nude reference.

## Completion gates

Source clip creation alone is incomplete. Require real bake PNGs, inspected/cleaned final frames, imported bank mappings, coherent clothing coverage and actual layered game playback. Discrete frame inspection catches static holes; continuous gameplay catches desynchronization, phase jumps, masks/order errors and marker mistakes. The plugin's source validation checks structure, not Unity rendering or actual asset quality.
