# Eight idles, eight walks, and requested action states

This document covers locomotion. Custom actions use their own durations, timed
poses, and playback rules from [action authoring](action-authoring.md). The
sixteen states below remain the locomotion foundation of the shared controller.

## Clips and controller

| Direction | Idle clip/state | Walk clip/state |
|---|---|---|
| South | IdleSouth | WalkSouth |
| SouthWest | IdleSouthWest | WalkSouthWest |
| West | IdleWest | WalkWest |
| NorthWest | IdleNorthWest | WalkNorthWest |
| North | IdleNorth | WalkNorth |
| NorthEast | IdleNorthEast | WalkNorthEast |
| East | IdleEast | WalkEast |
| SouthEast | IdleSouthEast | WalkSouthEast |

Keep all states on layer 0 named Base Layer. Default state is IdleSouth. Each
state plays its matching looping clip at speed 1 with Write Defaults off. Root
motion is off; Animator uses its normal update mode. The template driver calls
Animator.Play directly, so the starter controller needs no transition graph,
blend tree, or direction parameter. Do not add automatic transitions that
compete with the driver's normalized-time preservation.

Key the same bone property sets in every clip. Each direction keys its captured
rest positions and neutral rotations/scales, plus actual gait deltas. This prevents
a previous view's pose from persisting after a turn. Never key SpriteResolver
category/label data, SpriteRenderer sprites, or socket transforms.

Idle is a 1-second loop, with optional 0.25-pixel SpineUpper rise at 0.5 seconds
and matching negative Head compensation. Other bones stay in the authored
direction's neutral stance. Reduce breathing if fractional-pixel shimmer is visible.

Every walk lasts 0.5 seconds. Use exact keys at 0, 0.125, 0.25, 0.375, 0.5 seconds.
Contact A advances L leg and R arm along the facing vector. Contact B advances R
leg and L arm. Interpolate positions/angles continuously; physical limbs never
swap. Pelvis bob is 0, +1, 0, +1, 0 pixels, with negative head compensation.

## Starter curves

At 32 PPU, one pixel is 0.03125 Unity units. `stridePixels` is the L leg's Contact A
offset from its rest pose; R uses the opposite. At Contact B the offsets reverse.
Both passing poses use zero thigh/arm contact offset. Arms use the opposite leg's
offset times `armSwingRatio`. Foot L gets a small lift at Passing A; Foot R lifts
at Passing B. Small knee rotations accompany the lift. All values are authored
per direction, and all scales remain positive and unchanged.

The editor template intentionally generates modest starter motion with linear
keys, not a finished gait. SOUTH/NORTH restrict X stride to zero; W/E allow a
small horizontal stride; diagonal profiles start with roughly 1.414 pixels on
both axes. Larger horizontal stride is appropriate for side views only after
checking the pelvis-to-thigh overlap. Source art and camera perspective determine
final amplitudes. There is no IK or planted-foot solver in this first package.

## Camera depth versus forward motion

| View | Camera-side limb | Contact A depth | Contact B depth |
|---|---|---|---|
| South | depends on step | L leg near, R arm near | R leg near, L arm near |
| SouthWest | usually L | L side near | L side near |
| West | L | L side near | L side near |
| NorthWest | usually L | L side near | L side near |
| North | depends on step | R leg near, L arm near | L leg near, R arm near |
| NorthEast | usually R | R side near | R side near |
| East | R | R side near | R side near |
| SouthEast | usually R | R side near | R side near |

Diagonal fixed-side orders are a starting occlusion policy for a 3/4 camera.
Tune the four depth entries to the artwork; their purpose is to model depth,
not to dictate a universally correct projected overlap. Near-side and forward
stride are distinct, particularly in rear diagonals.

Use limb-group near leg +5/far leg −5 and near arm +10/far arm −10 relative to a
torso at 0. Within each limb group, upper segment 0, lower segment 1, hand/foot 2,
weapon 3 is a starting point. The default South/North group order changes at the
passing poses; side and diagonal orders stay stable until authored otherwise.

`gaitPhase` runs 0, 0.25, 0.5, 0.75, 1 in each walk. LateUpdate chooses the corresponding
walkDepth quadrant after Animator evaluation. The repeated 1 maps to Contact A.
Idle uses idleDepth. Depth does not use an independent timer, so turns can retain
the sampled gait phase.

## Other layers

Profile rendererOrders covers every binding; it is authoritative, not inferred
from the label suffix. Suggested non-limb orders:

| Piece | Front views | Rear views |
|---|---|---|
| Pelvis, Torso_Lower, Torso_Upper | −2, −1, 0 | −2, −1, 0 |
| Head | +20 | +20 |
| Hair_Back | −20 | +21 (when covering rear head) |
| Hair_Front | +21 | transparent or authored rear-compatible order |
| HeadEquipment | +22 | +22, adjusted to art |
| ChestEquipment | +1 | −1 or transparent if it is front-only |
| BackEquipment | −15 | +15 |
| WaistEquipment | +2 | +2, adjusted to art |

Diagonal art determines which hair/armor layers cover the body. A long weapon
may require a different order within its arm group; inspect both contacts.
Turning swaps the complete appearance/profile before the target clip evaluates.
Start without directional crossfades. Add blends only after you can handle
outgoing/incoming art, pose, and sorting coherently.
