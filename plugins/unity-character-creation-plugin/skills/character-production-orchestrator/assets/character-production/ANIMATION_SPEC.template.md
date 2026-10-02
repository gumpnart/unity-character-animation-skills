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
