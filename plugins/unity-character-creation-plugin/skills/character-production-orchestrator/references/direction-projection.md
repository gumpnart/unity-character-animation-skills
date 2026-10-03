# Direction artwork and source pose projection

Use one canonical identity and one skeleton across directions. This table describes neutral artwork and initial locomotion projection; action poses and occlusion are validated separately. Screen vectors use +X right and +Y up. A pixel editor with Y down needs conversion, and the actual Unity world's axes are recorded in CHARACTER_SPEC.md.

| Direction | Screen movement vector | Neutral master view | Forward-travel foot projection |
| --- | --- | --- | --- |
| South | (0, -1) | Face/front torso visible, slightly elevated camera | slightly lower; near camera and in front at contact |
| SouthWest | (-1, -1), normalized | Front and character side toward screen-left | modest leftward and downward travel, some depth |
| West | (-1, 0) | Profile facing screen-left | leftward travel; near/far limb order from side artwork |
| NorthWest | (-1, +1), normalized | Back and side toward screen-left | modest leftward/upward travel; forward-travel foot can be farther |
| North | (0, +1) | Back of head/body, no front face reused | slightly higher; farther from camera at forward contact |
| NorthEast | (+1, +1), normalized | Back and side toward screen-right | modest rightward/upward travel; forward-travel foot can be farther |
| East | (+1, 0) | Profile facing screen-right | rightward travel; near/far limb order from side artwork |
| SouthEast | (+1, -1), normalized | Front and character side toward screen-right | modest rightward/downward travel, some depth |

For South/North, depth projection dominates and horizontal leg separation stays small. For East/West, forward movement has a larger screen-horizontal component, but limb origins remain connected to the same pelvis. Diagonals combine the directional projection with the approved camera perspective; they are not mirrored South clips.

Distinguish **forward-travel leg** from **camera-near leg**. They coincide at South forward contact and can be opposite at North forward contact. The camera-near limb renders in front; do not automatically draw the forward-travel leg in front for every view. A side master records which physical arm/leg is near; keep that order except when actual motion justifies a documented change.

Capture each direction's local neutral transform values before animation. Keep fixed anatomical L/R names even when screen-left/right positions change. Directional equipment grip offsets and render order belong to that direction's specification. Reusing a bone hierarchy never guarantees identical SpriteSkin bind poses across independently imported art; validate binding compatibility or use rigid sprite pieces.

For new production, validate the South loop and equipment attachment first, then complete the requested coverage. For all-eight delivery, missing masters or clips remain blockers. Neutral art, authoring poses, exported frame banks and gameplay movement are distinct artifacts. Bake/clean each required direction and play its synchronized layers at runtime.

The bone rules in this guide apply to authoring. Runtime direction changes select complete cleaned frame sets on one clock; source rig crossfades do not drive default gameplay visuals. See [layered playback](layered-frame-playback.md).
