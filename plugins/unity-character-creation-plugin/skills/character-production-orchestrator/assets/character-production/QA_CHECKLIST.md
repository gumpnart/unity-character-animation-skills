# Character production QA — hybrid pipeline

Character / source and bank revisions / test scene / Unity version: TBD
Requested actions, directions and equipment coverage: TBD
Evidence paths / reviewer / date: TBD
Unchecked entries are pending, not proof of failure or success.

## Master and direction sources

- [ ] Canonical identity, measured proportions, palette, neutral pose and base outfit recorded.
- [ ] Simple fitted top/shorts/bare feet baseline or accepted existing outfit; no unintended master regeneration.
- [ ] Requested direction masters keep anatomical limb identity, camera and common anchor.
- [ ] Both arms/legs readable; each oblique view has its own source calibration.

## Source parts, import and rig

- [ ] 18 source pieces and extra clothing/equipment layers have safe hidden overlap at every joint.
- [ ] Pivots, imported mesh coverage, source crop offsets, PPU and alpha validated.
- [ ] Shared source hierarchy, actual binding paths and six sockets checked.
- [ ] Rigid pieces preferred; joint deformation/weighting minimal; scale changes controlled.
- [ ] Source pose keys, curve extrema and intermediate samples reviewed; no gaps or unexpected clipping.
- [ ] Walk physical leg identity preserved; opposite subtle arm swing and depth order correct.

## Bake and pixel cleanup

- [ ] Real deterministic exporter run in Unity; source states restored afterwards; raw outputs kept.
- [ ] Camera/canvas/PPU/common pivot consistent; loop samples exclude duplicate endpoint.
- [ ] Every layer/item uses identical pose times and bank revision.
- [ ] Occlusion-aware passes/masks or split passes match the complete equipped source render.
- [ ] EVERY final frame reviewed at native scale for gaps, detached parts, outline, pixel shape and grip.
- [ ] Cleanup preserves identity and registration; final frame differs from raw only through recorded edits.
- [ ] No independently generated frame identities or undocumented tight crops.

## Bank import, wardrobe and gameplay

- [ ] FRAME_BANK_SPEC matches actual file count, exact timestamps, labels and required pass coverage.
- [ ] Point filtering, mipmaps/compression, alpha, full-rect/pivot and atlas boundaries validated.
- [ ] Upper/lower garment replacement independent; body coverage and explicit empty slots verified.
- [ ] One action clock selects all pass sprites atomically; no independent per-layer Animator drift.
- [ ] Direction/equipment switch retains normalized phase and uses complete compatible banks.
- [ ] Missing-bank swap preserves last complete valid outfit/view and reports the missing selection.
- [ ] Weapon position/rotation stays aligned with the baked hand for every frame.
- [ ] Gameplay marker crossings, low FPS, interruption, one-shot completion and hold tested.
- [ ] Movement/input/physics and ground sorting remain separate from visual motion.
- [ ] At least two continuous walk cycles plus action/direction/equipment transitions rendered.

## Defect localization and release

- [ ] Compare source pose → raw bake → final cleaned PNG → runtime layered composite at the same time/index.
- [ ] Isolate locked Southwest walking, neutral SW and direction transitions when investigating diagonal gaps.
- [ ] Record actual failing renderer/pass/label/index, confirmed cause and discriminating test.
- [ ] Retest the failing frame and continuous playback after fixing its stage.
- [ ] Changed source/clip/item/layout revisions invalidate affected downstream banks and QA status.
- [ ] All requested coverage complete or missing work explicitly recorded; compilation is not visual QA.

| Check / incident | Input revision | Evidence / confirmed cause | Fix and retest | Status / blocker |
| --- | --- | --- | --- | --- |
| TBD | TBD | TBD | TBD | pending |
