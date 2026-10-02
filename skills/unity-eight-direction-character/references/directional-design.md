# Eight-direction design

## Shared rig and art

Keep the same 21 bone Transform paths in every direction. Bone rest positions can
change in directional clips, but anatomical L/R identity and animated paths stay
constant. This first slice uses one rigid prefab, one SpriteLibrary, and one
Animator. Art layers may hide naturally behind another limb; don't change identity
or remove the far limb to avoid drawing it.

Author eight sets of the 18 base parts. Add eight views of each equipped item.
Use overlap and consistent PPU/pivots so turns and rotations do not expose holes.
Direction-specific transparent labels are useful for invisible facial details,
hair layers, and empty sockets, while keeping the resolver category bound.

## Facing selection

Inputs are screen-space/camera-projected vectors: +X right, +Y up. Project world
velocity onto camera right/up before passing it to the driver. Do not mistake
world Z for screen Y in an isometric or tilted camera.

Angles use `atan2(y, x)`, in degrees. Exact boundary ties choose the increasing
angle's sector; hysteresis normally prevents repeated flips at the boundary.

| Direction | Center | Sector before hysteresis |
|---|---|---|
| East | 0° | [−22.5°, 22.5°) |
| NorthEast | 45° | [22.5°, 67.5°) |
| North | 90° | [67.5°, 112.5°) |
| NorthWest | 135° | [112.5°, 157.5°) |
| West | 180° | [157.5°, 202.5°) |
| SouthWest | 225° | [202.5°, 247.5°) |
| South | 270° | [247.5°, 292.5°) |
| SouthEast | 315° | [292.5°, 337.5°) |

Keep last facing below the movement dead zone. A 5° angular hysteresis is a useful
starter: keep the current direction while the input stays within 27.5° of its
center. A turn larger than that chooses the closest 45° sector directly. Reject
nonfinite input and do not let NaN choose a direction.

## Runtime turn

1. Choose desired facing/movement state from the projected input.
2. Verify the target idle/walk state exists on Animator layer 0.
3. Build one appearance plan from the direction defaults plus currently equipped
   item variants. Verify every binding's category, label, SpriteRenderer, shared
   library, fixed sorting order, and applicable weapon socket before applying it.
4. Commit labels, fixed sorting orders, socket calibration, active profile, and
   target state in the same Update. Depth groups update in LateUpdate.
5. For Walk → Walk turns, retain the current state's fractional normalized time.
   For Idle → Walk, start Contact A. For Walk → Idle, use the target settled idle.

The template uses direct `Animator.Play` with zero blends, instead of a blend tree
that could interpolate incompatible front/back art. It owns layer 0 and receives
movement requests before its Update. Reject incomplete turns while retaining the
previous visual facing; movement can continue along the requested world path.
The movement caller owns whether an appearance failure should stop world motion.

Animated gaitPhase is shared by all walk clips. Fixed sorting orders and labels
are profile data; Animator does not write them. Preserve phase as a pose parameter
when turning. A phase reset at every direction change would cause repeated steps.

## Next features

Once the eight-direction acceptance matrix passes, extend each profile with
run/attack/hit animations, compatible skin parts, or a directional action socket.
Keep animation paths and item IDs stable. Do not add an inventory backend, IK,
combat, networking, or generalized content system solely to demonstrate facing.
