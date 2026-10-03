# Canonical layered frame-bank specification

Character ID / CHARACTER_SPEC revision: TBD
Revision / status: 0 / draft
pipeline_mode: hybrid-baked-frames
Actual bank metadata schema / asset type / driver implementation path: TBD
This is a production contract template, not a populated asset or executable exporter. Fill values from actual Unity assets and exported files. Never mark coverage complete from this template alone.

## Shared rendering and import contract

Native canvas width / height in pixels: TBD
PPU / coordinate origin / ground anchor in pixels / normalized sprite pivot: TBD
Camera projection / orthographic size / resolution / lighting: TBD
Alpha / color space / shader / background transparency: TBD
Filtering: Point; mipmaps disabled; compression policy: TBD
Sprite mesh: Full Rect unless an explicitly validated alternative is recorded
Canvas cropping: no independent tight crop; every pass/frame uses the same registered canvas
Atlas padding / transparency and edge tests: TBD
Pixel cleanup tools / raw and final directories / reviewer evidence: TBD
Runtime render pass layout / category IDs / layout revision: TBD
SortingGroup / ground sorting owner / pass order owner: TBD
Body and clothing ownership / replacement region and masks: TBD

## Action bank (repeat per action/direction)

Action ID / direction / bank ID / metadata path: TBD
Source rig / rest / clip / camera / render layout revisions: TBD
Duration seconds / loop, one-shot or hold mode: TBD
Requested export FPS / effective FPS / exact sample timestamps: TBD
Frame count N / valid indices 0..N-1: TBD
Loop: sample [0,duration), excluding duplicated endpoint
One-shot: record endpoint inclusion, completion time and last-frame hold explicitly
Sampling formula or timestamp lookup / normalized phase mapping: TBD
Raw composite / raw pass / final pass directories and naming: TBD

| Index | Sample time seconds | Source pose or sample evidence | Full composite reference | Raw pass files | Cleaned pass files | Accepted evidence |
| --- | --- | --- | --- | --- | --- | --- |
| TBD | TBD | TBD | TBD | TBD | TBD | pending |

## Appearance/item bank (repeat per appearance/action/direction)

Stable appearance or item ID / slot / revision: TBD
Compatible action-bank ID / timing revision / render layout revision: TBD
Required pass IDs / optional pass IDs / explicit empty labels: TBD
SpriteLibrary asset / category-label convention: TBD
Suggested label: Appearance_Action_Direction_fNNN (record the actual chosen schema)
Replacement regions / occluder masks / split front-back pass policy: TBD

| Pass | Frame index | Sprite asset path / GUID or category-label | Canvas / PPU / anchor checked | Alpha/occlusion check | Status |
| --- | --- | --- | --- | --- | --- |
| TBD | TBD | TBD | pending | pending | pending |

Record all N entries for every required pass; transparent passes still need explicit registered selections. A single equipment PNG is not a complete action bank. Matching frame counts alone do not establish matching poses or timing.

## Gameplay markers and optional per-frame anchors

| Marker ID | Action time / occurrence policy | Subscriber | Loop/interrupt/low-FPS crossing evidence |
| --- | --- | --- | --- |
| TBD | TBD | TBD | pending |

| Frame index | Anchor ID | Position / rotation and coordinate space | Source grip / runtime evidence |
| --- | --- | --- | --- |
| TBD | TBD | TBD | pending |

Evaluate marker crossings on the action timeline, not only equality with a frame number. Reconstruct anchors with the common pivot/PPU. Gameplay hitboxes/VFX have explicit ownership and do not automatically follow a baked image.

## Runtime and coverage acceptance

Single clock / state owner / driver paths: TBD
Atomic frame, direction and appearance selection implementation: TBD
Missing-bank behavior: retain last complete valid view; report missing data
Required actions/directions/items / actually complete coverage: TBD
Every exported frame reviewed / native-scale evidence: pending / TBD
Final layered composite vs equipped source composite: pending / TBD
Continuous playback, loop seam, direction switch and equipment swap: pending / TBD
Source change invalidation and stale bank list: TBD
Confirmed defect stage: source pose / raw bake / pixel cleanup / import / runtime selection (choose using evidence)
Revision history / blockers / next task: TBD
