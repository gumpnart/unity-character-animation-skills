# SouthWest

Movement is **down-left** on screen. Use a **front-left elevated 3/4** with the same slight
camera elevation as the other directions. State/clip names are IdleSouthWest and
WalkSouthWest; example labels are BaseSouthWest, LeatherArmorSouthWest, SwordSouthWest.

## Artwork and rest pose

Author front and left-facing side of face/torso; keep the far eye/cheek consistent with the view. Keep 18 part categories, anatomical L/R identities, and the exact
shared bone paths. Direction-specific rest positions/pivots establish the view;
do not rotate or flip a SOUTH drawing into this view. Visibility check: far arm and leg remain legible through staggered pivots and overlap, rather than swapping identities.

## Gait

Contact A: L forward down-left, R back up-right; physical R arm leads opposite physical L leg.
Contact B: R forward down-left, L back up-right; physical L arm leads opposite physical R leg.
Passing A at 0.125 moves L back and R forward; Passing B at 0.375 reverses it.
Contact A repeats at 0.500. Both passing poses add one pixel of body bob, with
head compensation. Front/forward names always describe actual bone motion.

Use a starter L-contact stride vector of **(−1.414, −1.414) pixels**, relative to rest,
and its negative for R; reverse those offsets for Contact B. Keep arm amplitude
about 30% of the leg offset or smaller. Tune stride/lift/knee bends against the
view's silhouette, joint overlap, and camera projection. This vector is an
initial authoring value, not a finished animation or a directional sprite frame.

## Depth and equipment

Starter policy: **L side near, R side far**. Profile walkDepth has Contact A, Passing A, Contact B,
and Passing B entries. In oblique views the camera-side limb can stay nearer even
when the other leg is forward along movement; tune the four entries if authored
occlusion requires a change. Never change L/R bone identity to correct layering.

Equipment checks: L arm/leg group usually near; right-hand weapon is on the far side and can cross behind torso; author its leftward blade view. Weapon_R stays on Hand_R and Weapon_L on Hand_L.
Directional grip offsets and per-renderer profile orders handle projection.

## Acceptance

Scrub both contacts and passing poses; then run ten loops. Verify identity,
counter-swing, no joint gaps, stable head, correct complete-limb depth, and no loop
pop. Equip both test armors and both weapons while walking, turn to each adjacent
view mid-stride, and verify labels, grip, and gait phase remain coherent. Check
this view from idle and when arriving from its opposite view as well.
