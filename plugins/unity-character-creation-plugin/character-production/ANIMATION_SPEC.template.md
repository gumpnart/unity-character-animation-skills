# Animation and runtime action specification

Character ID / master revision / rig revision: TBD
Revision / status: 0 / draft
Requested action/direction scope: TBD
Transform binding root / rest restoration / write-defaults policy: TBD
Animator path / runtime owner / parameter contract: TBD

## Action inventory and coverage

Arbitrary named action IDs are supported; the list is not a fixed enum.

| Action ID | Requested directions | Created clip paths | Validated directions | Status / evidence / missing coverage |
| --- | --- | --- | --- | --- |
| Idle | South initially | TBD | none | pending |
| Walk | South initially | TBD | none | pending |

## Action record (repeat per action)

Action ID / display name / family: TBD
Source rig / direction rest revisions: TBD
Duration / sample rate: TBD
Playback: loop / one-shot / hold (choose)
Pose grammar: TBD, specific to this action
Root-motion / gameplay movement ownership: TBD
Facing lock / requested movement while active: TBD
Interrupt policy / priority / cancel or combo windows: TBD
Completion: latest locomotion / next action / terminal hold (choose)
Markers and gameplay subscribers: TBD (signals do not automatically apply effects)
Equipment and required grips: TBD

| Time (seconds) | Phase | Direction | Bone rest-relative poses | Foot/hand targets and body/head motion | Draw order | Marker |
| --- | --- | --- | --- | --- | --- | --- |
| TBD | TBD | TBD | TBD | TBD | TBD | TBD |

Loop seam / final pose / transition rest restoration: TBD
Clip assets / controller states / exact binding checks: TBD
Visual preview / joint checks / gameplay event evidence: TBD
Status / blocker / revision history: draft / TBD / TBD

## Controller and runtime transitions

| Trigger or input | From → to | Facing/movement behavior | Interruption / completion | Evidence |
| --- | --- | --- | --- | --- |
| TBD | TBD | TBD | TBD | TBD |

Record Idle↔Walk, actions→latest locomotion, cast hold/release, hit/interrupt and death hold only when required clips exist. WalkSouth baseline is 0/.125/.250/.375/.500 seconds; it does not constrain other actions.

## Moving-silhouette evidence (repeat per action/direction)

Fixed-direction neutral / continuous-playback test: TBD
Exact phase keys / motion-curve extrema / sampled intermediate times: TBD
Sampling step / effective playback speed / runtime frame rate: TBD
Parent joint anchors / segment-length changes / allowed scale changes: TBD
Required renderer/sprite inventory at first failure: TBD
Expected silhouette / intentional occlusion / unexpected gaps: TBD
Direction transitions tested at contact and passing: TBD
Native-scale capture sequence / before-after evidence paths: TBD
Confirmed cause / discriminating test / fix / retest result: TBD

For the 0.5-second walk include exact .000/.125/.250/.375/.500 keys, samples at intervals no greater than 1/60 second, curve extrema and two continuous rendered runtime cycles. Apply appropriate effective-rate sampling to other actions. A few keyframe captures or compilation alone do not validate the moving silhouette.

## v2 source-to-frame action record (repeat per action/direction)

Source AnimationClip path / authoring rig, rest and item revisions: TBD
Source duration and keys / source pose validation: TBD
Bake scene and exporter implementation path / Unity execution evidence: TBD
Fixed camera / render pass and occluder layout revision: TBD
Requested export FPS / actual FPS / sample times / frame count: TBD
Loop sampling: t_i = i * duration / N, i = 0..N-1; do not export a duplicate endpoint
One-shot sampling and terminal hold: explicitly record endpoint policy
WalkSouth reference: 5 source keys at 0/.125/.250/.375/.500; proposed 12 FPS gives 6 unique loop frames at 0/1⁄12/2⁄12/3⁄12/4⁄12/5⁄12, duration .5 seconds
These are different counts. Choose FPS for the approved visual style; do not promise smoothness from FPS alone.
Raw bank / final cleaned bank / metadata paths: TBD
Pixel cleanup revisions / every-frame acceptance evidence: TBD
Runtime marker times, interruption policy and final hold: TBD
One action timeline drives Body/Hair/Clothing/Armor/Weapon; per-layer timing must not drift.
Every-frame bank/import/runtime acceptance: pending

The 1/60-second sampling requirement above diagnoses the continuous source rig. Final banks require review of EVERY exported frame plus continuous layered runtime playback. FRAME_BANK_SPEC.md records authoritative output scheduling and layer alignment; source curves are not runtime bone tracks in the default pipeline.
