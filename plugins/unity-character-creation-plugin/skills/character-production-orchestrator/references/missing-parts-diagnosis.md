# Hybrid defect localization

First compare four artifacts: source rig pose → raw bake PNG → final cleaned frame → runtime layered composite. If the final PNG/composite is clean but gameplay breaks, inspect frame index, item/action/direction mapping, canvas/pivot, replacement masks and pass order before changing the source rig. Source SpriteSkin/bone checks below apply to authoring; default runtime has no live limb deformation. Every final exported frame requires inspection.

# Diagnose missing parts and motion gaps

Use this protocol when a character is complete in one pose/direction but develops missing pixels, detached joints, disappearing limbs or silhouette holes during motion. A report of gaps during Southwest walking is a symptom, not proof of a particular cause. Collect the affected direction, clip, phase, equipment, actual frame/capture, renderer state and rig revision before assigning a root cause.

## Recovery entry

Run the invoking skill's fresh Unity plugin gate. Find the existing game project and prefab; preserve the canonical master and valid stages. Reproduce the defect in a duplicate/test scene. Do not restart character generation or expand to more directions while the affected required view is broken. If the Editor/artwork is inaccessible, report the observed symptom and plausible causes as hypotheses; the root cause and game fix remain unverified.

Start with the base body, empty equipment and one fixed direction. Disable automatic facing changes for the experiment. For Southwest, compare:

1. South neutral and walking.
2. Southwest neutral with Animator motion paused, SW artwork and SW rest calibration applied.
3. Fixed Southwest for a whole walk cycle.
4. South→Southwest and West→Southwest during contact and passing, then the reverse transitions.
5. Add the active equipment and rerun only after the base body passes.

Use the actual gameplay camera at native pixel scale, with a contrasting background and saved captures. Scene view appearance or a successful compile is not rendered-game evidence. Record whether the failure exists in neutral, fixed-direction motion, direction switching or equipment only.

## Decision table

| Observation | Hypothesis to test | Discriminating check | Correct owner / fix |
| --- | --- | --- | --- |
| SW neutral already has holes | Incomplete SW art, crop/pivot error, missing labels or bind mismatch | Reassemble source parts in the canonical canvas; inspect the imported neutral pose and each resolver result | rig-ready-parts / unity-asset-import / unity-skeleton-rig; fix the responsible source or calibration |
| Neutral passes; a joint opens at a repeatable gait phase | Insufficient overlap for SW, wrong pivot, joint-origin translation or inherited scale | Pose the failing joint manually at that time; compare source overlap, parent/child joint anchors and local transforms | rig-ready-parts for missing pixels; unity-skeleton-rig or animation-clip-authoring for pivot/curve defects |
| Whole part vanishes or collapses | Null sprite, disabled object/renderer, alpha/scale curve, invalid SpriteSkin data or culling/mask | Inspect sprite, active/enabled flags, effective color/scale, bones, mesh, bounds and camera/mask state at the failing time | import/rig/clip/runtime stage according to the failed invariant |
| Stable SW passes; switching to SW fails | Partial library update, missing SW labels, stale sorting/rest values or incompatible blended poses | Log all intended category/label results and state/phase before and after one facing change; compare to locked SW | directional-animation-system / modular-equipment-system / animator-controller; apply validated mappings coherently |
| Sprite exists and is valid but appears hidden | Wrong near/far part ordering or unintended occlusion | Temporarily isolate that renderer or use a diagnostic tint/order in the duplicate scene; restore afterward | direction-master / directional-animation-system; correct the SW visibility and sorting contract |
| Only an equipped variant fails | Coverage, pivot, mesh/bone compatibility or grip mismatch | Repeat with base body and empty slot, then equip the same item at the same phase | modular-equipment-system; repair that item's direction/rig data |
| Source/imported alpha contains the missing region | Missing hidden artwork or tight mesh excluding opaque/overlap pixels | Inspect source texture alpha and sprite mesh independently of the Animator | rig-ready-parts / unity-asset-import; complete art or regenerate suitable geometry |

Expected occlusion is not missing artwork. Define the approved SW silhouette and which part regions must remain readable; intentionally occluded rear parts and empty optional equipment are allowed. Do not demand all 18 pieces be fully visible in every view.

## Renderer and rig inventory

For every expected body part at neutral and at the first failing time, record:

- Actual GameObject/binding path; anatomical identity; expected visibility or intentional occlusion.
- Sprite asset/path and resolver category/label; null or unresolved selections.
- activeInHierarchy, renderer enabled, effective alpha and material/mask behavior.
- Local and inherited transform scale, position and rotation; animation bindings affecting visibility, sprite, alpha or scale.
- Sorting layer/order, enclosing SortingGroup, near/far relationship and camera position/clip range.
- For SpriteSkin: rootBone, ordered bone bindings, rest/bind poses, sprite mesh/weights, deformation state and bounds.

Use the actual Editor/package API version and discovered commands. `renderer.isVisible` alone is not evidence: it can include non-game cameras and cannot distinguish occlusion. A non-null sprite does not prove SpriteSkin deformation or final rendering is valid.

Inspect each expected opaque source region against imported geometry. Tight meshes are not intrinsically wrong; missing triangles, excluded overlap regions or invalid skin weights are. Verify bounds after deformation instead of guessing a large universal bound. Do not force every renderer to the foreground or disable culling globally as a production fix.

## Directional attachment invariants

Capture a validated rest pose for EACH direction; a common hierarchy does not imply common local rest transforms or compatible sprite bind data. Use the SW calibration when authoring WalkSouthWest. Copy semantic timing from a valid base walk, then author the projected SW poses.

Keep upper-arm origins attached to their Shoulder bones and thigh origins attached to Pelvis. Child joints follow their parent hierarchy and approved pivots. Avoid translating the thigh and shin and foot independently along the full movement vector; that can detach the chain or apply depth motion several times. Rigid limb articulation primarily uses rotations. Intentional local translation/scale must stay within the validated overlap/contact range and be recorded explicitly.

Ground locomotion belongs to the character gameplay root. Visual bob belongs inside the visual rig. Foot/depth targets are projected and solved consistently against the shared chain; do not add the gameplay diagonal displacement to every limb. Do not introduce negative-scale mirroring or swap anatomical L/R to hide the defect.

An interpolation curve can overshoot between valid keys. Inspect rotation and position extrema, tangents, joint origins and any scale curves in the failing interval. Preserve canonical segment lengths except for a specifically approved and validated projection change. A direction-specific sprite/pivot swap must not leave the renderer with another direction's attachment offset.

## Southwest-specific projection and visibility

Southwest is a front-side oblique view with modest screen-left and downward movement. Its overlap, visible-side silhouette and near/far draw order must come from the SW source artwork and rest pose. Validated South overlaps do not automatically cover SW joint rotations. Never infer the camera-near physical limb solely from an `_L`/`_R` suffix or copy South ordering blindly.

For fixed SW walking, preserve physical Leg A=L and Leg B=R, opposite-arm swing and a coherent half-cycle. The front-travel leg and camera-near leg are different concepts. Record which limb regions should be visible at each contact/passing pose and when ordering changes. Keep both arms/legs readable according to the approved perspective, without widening the stance to conceal joint gaps.

Direction switching must validate the complete body/item category-label mapping and compatible rig calibration before applying it. Update artwork, rest/clip selection and part ordering before the next rendered frame, preserving gait phase. If a required label is missing, report the mapping defect and retain the last complete valid view; do not clear individual body parts. Restore action-animated sort/scale/visibility properties deliberately.

Avoid blending transform poses calibrated to different sprite directions until that blend has rendered evidence. Start with explicit coherent direction variants. Do not let a transition interpolate South/West curves while showing uncalibrated SW parts. Test any chosen crossfade implementation, not only the clips in isolation.

## Mandatory evidence gate

For an affected action/direction, save actual screenshots/capture sequence and a phase/state log for:

- Imported neutral reconstruction and isolated joint rotation extrema.
- Every named action phase/key, endpoints/loop seam and motion-curve extrema.
- Intermediate times at intervals no greater than 1/60 second for the initial 0.5-second walk, plus exact 0.000/0.125/0.250/0.375/0.500 keys. For other actions sample at their effective playback/update rate or finer; record the chosen step and playback speed.
- Rendered continuous playback for at least two walk cycles, including actual runtime motion.
- Contact/passing direction switches and the affected equipment combination.

Discrete samples alone cannot prove every continuous pose correct. Use continuous playback and inspect suspicious intervals more densely. These source captures validate the editable authoring clip. In the hybrid pipeline, separately inspect every raw/cleaned exported frame and the synchronized runtime composite; gameplay plays approved final images.

Pass requires zero unexplained joint gaps and zero unexpectedly missing required regions/parts across the recorded requested coverage. Compare to the approved silhouette; natural background spaces between separate limbs are not defects. Existing stylized cut-outs require a recorded design reason. If renders, source inspection or inventories are unavailable, mark blocked/pending rather than validated.

Write observed cause, discriminating check, affected revisions and before/after evidence into QA_CHECKLIST.md. Record per-direction anchors/overlap/visibility in DIRECTION_SPEC.md and phase sampling/curve extrema in ANIMATION_SPEC.md. A fix invalidates the affected checks; retest before setting validated or expanding direction coverage. New evidence fields must be added to existing canonical specs without replacing them with templates.

## Report the conclusion accurately

Distinguish symptom, suspected cause, confirmed cause, applied fix and retest result. For example, "SW knee gap at t=0.18; confirmed source overlap insufficient at the approved bend; shin art extended; fixed-SW and South↔SW tests passed with captures" is evidence-backed. "Looks like rigging" without a discriminating check is a hypothesis. Plugin workflow improvements are not proof that the user's current prefab has been repaired.
