# Character production QA

Character ID / scope / revision: TBD
Referenced master / direction / rig / animation / equipment revisions: TBD
Overall status: pending
Evidence directory / scene / editor/package versions: TBD

Use pending / pass / fail / blocked / stale per check. A pass requires an actual observation and evidence; a planned test remains pending. Mark checks outside the requested scope N/A with a reason.

| Check | Status | Artifact revision / test mode | Evidence path / observed result |
| --- | --- | --- | --- |
| Canonical master identity, proportions and palette | pending | TBD | TBD |
| Requested neutral direction coverage and anatomical L/R | pending | TBD | TBD |
| Parts reconstruct the master and have hidden overlaps | pending | TBD | TBD |
| Imported PPU, filtering, alpha, pivots and naming | pending | TBD | TBD |
| Shared skeleton, prefab bindings and six sockets | pending | TBD | TBD |
| Joint ranges: no gaps, rubber deformation or pixel distortion | pending | TBD | TBD |
| IdleSouth stable neutral and loop seam | pending | TBD | TBD |
| WalkSouth five keys, real leg identities, opposite subtle arms | pending | TBD | TBD |
| Front/rear depth, body bob and stable head | pending | TBD | TBD |
| Requested actions use their own phase grammar and timings | pending | TBD | TBD |
| Action/direction coverage, markers, loop/final poses | pending | TBD | TBD |
| Equipment swaps retain shared animations | pending | TBD | TBD |
| Attached test weapon and two-hand grip where needed | pending | TBD | TBD |
| Direction changes preserve appearance, timing and sorting | pending | TBD | TBD |
| Animator idle/walk/action/interrupt/hold transitions | pending | TBD | TBD |
| Runtime input, normalized diagonals, retained/locked facing | pending | TBD | TBD |
| Ground sorting across characters and props | pending | TBD | TBD |
| Gameplay effects have one owner and correct marker timing | pending | TBD | TBD |
| Compilation and console clean for requested scope | pending | TBD | TBD |
| Play mode, scene reload and multiple-instance behavior | pending | TBD | TBD |

## Defect log

| ID | Reproduction: direction/action/equipment | Expected vs actual | Owner stage | Severity / status | Fix revision / retest evidence |
| --- | --- | --- | --- | --- | --- |
| TBD | TBD | TBD | TBD | TBD | TBD |

## Scope verdict and next task

Validated coverage: none yet
Missing / blocked / stale coverage: TBD
Native-scale captures / runtime logs: TBD
Next owner stage and concrete task: TBD

## Missing-part and oblique-walk incident record

Direction / clip / equipment / first failing time: TBD
Failure class: neutral / fixed-direction motion / direction switch / equipment (choose)
Expected silhouette and intentionally occluded regions: TBD
Required renderer inventory / source and imported sprite paths: TBD
Observed symptom / suspected cause: TBD
Discriminating check / confirmed cause: TBD
Applied source, pivot, binding, pose, resolver or sorting fix: TBD
Sampling step / playback speed / curve extrema / tested revisions: TBD
Before/after captures / continuous runtime evidence: TBD
Remaining unexplained gaps / missing required parts: TBD
Retest outcome / next owner stage: pending / TBD

| Reproduction condition | Status | Clip times / revision | Native-scale evidence / observation |
| --- | --- | --- | --- |
| South reference neutral / walk | pending | TBD | TBD |
| Affected direction neutral (e.g. Southwest) | pending | TBD | TBD |
| Affected direction locked for entire walk cycles | pending | TBD | TBD |
| Joint and curve extrema plus intermediate poses | pending | TBD | TBD |
| South/West ↔ Southwest at contact and passing, when affected | pending | TBD | TBD |
| Base body / empty equipment versus affected loadout | pending | TBD | TBD |

Pass requires zero unexplained joint gaps or unexpectedly missing required regions across the recorded scope. Natural spaces between limbs and documented intentional occlusion are allowed. Missing renders or inaccessible Unity keep the test blocked/pending. Protocol/documentation checks are not a confirmed fix of a game prefab.
