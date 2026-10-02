# West

Movement is **left** on screen. Use a **left-facing elevated side view** with the same slight
camera elevation as the other directions. State/clip names are IdleWest and
WalkWest; example labels are BaseWest, LeatherArmorWest, SwordWest.

## Artwork and rest pose

Author left profile of face, shoulder and feet; far limbs are physically R. Keep 18 part categories, anatomical L/R identities, and the exact
shared bone paths. Direction-specific rest positions/pivots establish the view;
do not rotate or flip a SOUTH drawing into this view. Visibility check: stride can spread horizontally more than SOUTH; separate knee/foot silhouettes without splaying the hips.

## Gait

Contact A: L forward left, R rear right; physical R arm leads opposite physical L leg.
Contact B: R forward left, L rear right; physical L arm leads opposite physical R leg.
Passing A at 0.125 moves L back and R forward; Passing B at 0.375 reverses it.
Contact A repeats at 0.500. Both passing poses add one pixel of body bob, with
head compensation. Front/forward names always describe actual bone motion.

Use a starter L-contact stride vector of **(−2, 0) pixels**, relative to rest,
and its negative for R; reverse those offsets for Contact B. Keep arm amplitude
about 30% of the leg offset or smaller. Tune stride/lift/knee bends against the
view's silhouette, joint overlap, and camera projection. This vector is an
initial authoring value, not a finished animation or a directional sprite frame.

## Depth and equipment

Starter policy: **L side near, R side far**. Profile walkDepth has Contact A, Passing A, Contact B,
and Passing B entries. In oblique views the camera-side limb can stay nearer even
when the other leg is forward along movement; tune the four entries if authored
occlusion requires a change. Never change L/R bone identity to correct layering.

Equipment checks: right-hand weapon remains far with Hand_R; do not move it onto the visible near hand to make it readable. Weapon_R stays on Hand_R and Weapon_L on Hand_L.
Directional grip offsets and per-renderer profile orders handle projection.

## Acceptance

Scrub both contacts and passing poses; then run ten loops. Verify identity,
counter-swing, no joint gaps, stable head, correct complete-limb depth, and no loop
pop. Equip both test armors and both weapons while walking, turn to each adjacent
view mid-stride, and verify labels, grip, and gait phase remain coherent. Check
this view from idle and when arriving from its opposite view as well.
