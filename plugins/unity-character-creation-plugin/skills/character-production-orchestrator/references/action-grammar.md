# Action grammar, authoring and baked timing

Actions are data identified by an arbitrary stable string such as `SwordLightAttack`, `Mining`, or `Celebrate`; the examples below are suggestions, not an enum. Specify pose grammar per action, any number of keys, configurable durations, loops/holds, marker events, facing lock, root motion and completion behavior. Mechanical defaults are proposals until recorded and tested.

| Family | Example actions | Suggested phases |
| --- | --- | --- |
| Locomotion | Idle, Walk, Run, Sprint | idle breathe/settle or contact → passing → opposite contact → passing; run may include flight |
| Movement | Jump, Landing, Dodge, Roll | crouch → launch → airborne → contact → settle; or anticipation → evade/tuck → recovery |
| Combat | Attack, HeavyAttack, Combo, SkillAttack, BowAttack, TwoHandAttack | anticipation → acceleration → contact → follow-through → recovery |
| Bow variation | BowAttack | raise/nock → draw → aim/hold → release → recovery |
| Magic | Cast, Channel, Release | anticipation → channel/hold → release → recovery |
| Reaction | Hit, Knockback, Death | impact → recoil → settle; knockback displacement; death collapse → terminal hold |
| Interaction | Interact, UseItem, Emote | reach/prepare → interaction/hold → complete → recover or explicit loop |

A combo defines attacks and input windows, not just a longer Walk-like clip. A cast hold can be an explicit state/loop; release begins on the gameplay request. Death defaults to one-shot terminal hold. Jump height/world physics is a gameplay contract unless animation root motion is explicitly chosen; do not apply the same displacement twice.

## South walk baseline

Anatomical Leg A = Leg_L; Leg B = Leg_R (record this assignment). Physical identities never swap. The opposite arm advances with the front leg; use subtle swing and limited horizontal separation.

| Time (s) | Pose | Front leg | Opposite front arm | Pelvis/body Y pixels |
| --- | --- | --- | --- | --- |
| 0.000 | Contact A | A / L | B / R | 0 |
| 0.125 | Passing A | A moves backward; B forward | reverse subtly | +1 |
| 0.250 | Contact B | B / R | A / L | 0 |
| 0.375 | Passing B | B moves backward; A forward | reverse subtly | +1 |
| 0.500 | Contact A | A / L | B / R | 0 |

For the South projection the front foot/leg is lower on screen and drawn in front; rear is higher and drawn behind. Optional small scale differences must preserve crispness and be validated. Convert pixels to Unity units using canonical PPU. Keep the head comparatively stable with reduced bob/compensation appropriate to the rig. This five-key, 0.5s loop is the initial WalkSouth baseline only; other actions have their own pose grammar.

IdleSouth uses a stable neutral pose with minimal breathing/weight motion compatible with the pixel style. Avoid large head sway or rubber-like torso/limb deformation. Specify its own duration and loop seam.

## Any-action pose table

Record action/direction/duration, then table rows with: time or normalized time, phase, bone rest-relative position/rotation, feet/hands/contact/grip targets, body/head offset, render order and markers. Sparse edits must restore every bound property at action transitions. The first/last pose of a loop matches; a one-shot defines its final pose and return/hold rule.

Animation events are signals such as `Contact`, `Release`, `Footstep`, `ComboWindowOpen`, `ComboWindowClose`; gameplay validates effects. Never claim effects occur merely because the event exists. Direction variants share semantic phase timing/marker IDs unless documented differences are intentional.

## Example SwordLightAttack / South (proposed timing)

| Time | Phase | Pose intent | Marker |
| --- | --- | --- | --- |
| 0.00 | Anticipation | neutral toward compact wind-up, stable support foot | — |
| 0.12 | Acceleration | subtle torso turn and sword hand travel, grip locked | — |
| 0.20 | Contact | readable attack line, opposite hand balances | Contact |
| 0.30 | Follow-through | limited overshoot, no joint gaps | — |
| 0.50 | Recovery | restore directional neutral, return to latest idle/walk | Complete |

These are starting values, not generated art or a prevalidated clip. Run native-scale previews and refine to the user's requested action. Two-handed actions pose the secondary hand to the weapon grip; do not swap hand identities. Custom actions such as digging, fishing, climbing or dance follow the same specification mechanism.

## Hybrid frame output

The five-key walk table is the editable skeletal source, not a requirement to export exactly five images. Record a common bake sample schedule in FRAME_BANK_SPEC.md (for example six loop frames at 12 FPS for a 0.5-second walk). Inspect and clean every final frame. All body/item passes sample the same source pose times; semantic action markers stay on the shared timeline and are dispatched even when a render update skips frames. See [bake and cleanup](bake-and-cleanup.md).
