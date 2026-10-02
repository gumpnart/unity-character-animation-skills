# Create any named skeletal action

The action system accepts a free action ID and an arbitrary pose timeline. It
does not enumerate action types. Attack, cast, dodge, jump, eat, sit, mine, carry,
wave, recoil, death, and invented actions all use the same authoring contract.
First complete the mandatory [Unity plugin handshake](unity-plugin-integration.md)
on this invocation. Unavailable plugin readiness blocks the action workflow.
See [example specifications](action-examples.md) for different timing/playback modes.
This is character skeletal animation; it does not claim to generate unrelated
scene/VFX animation or missing source artwork automatically.

## From description to a concrete specification

Read the request and existing project. Produce a compact specification containing:

- Safe actionId, display intent, requested character and direction scope.
- Duration per direction; normally share duration unless projection or design
  specifically calls for a difference.
- One-shot/loop, interruptibility, equipment-swap permission, and return/hold policy.
- Ordered pose times and their intent: anticipation, main movement, contact/cue,
  recovery, settled endpoint, or any phases suited to the requested action.
- Bones involved, anatomical hand/foot identities, equipment grip requirements.
- Named markers and their times; these are cues, not gameplay implementations.
- Per-key limb/renderer ordering for weapons or limbs crossing camera depth.
- World-motion policy, entry/exit stance, and artwork/overlap requirements.

Default to all eight directions, a full-body one-shot, locked visual facing,
interruptible playback, no equipment swap during the action, and return to current
idle/walk. Choose duration based on the described movement. State those choices
and proceed when they are reversible and adequately supported. A requested loop,
held pose, exact cue, or limited direction set takes precedence.

Ask only when an unresolved detail blocks correct work, such as which hand must
hold an existing asymmetric tool. Continue independent art/rig inspection while
waiting. Do not ask for routine approval of durations or internal action IDs.

## Data and curves

Create one CharacterActionDefinition ScriptableObject per requested action, with
one ActionDirectionTrack per supported direction. It initially contains eight
neutral placeholders. Replace those placeholders with actual choreography; an
asset with only rest keys is not a completed requested action.

Each direction track defines:

| Field | Meaning |
|---|---|
| direction | One supported physical facing |
| duration | Positive total seconds, chosen for this action |
| curves | Linear (default) or SmoothClamped |
| poses | Two or more strictly increasing pose times, including 0 and duration |
| markers | Named, ordered cue times strictly inside the clip |

Each pose has sparse ActionBoneOffset entries: exact bone path, local XY pixel
offset relative to that direction's captured rest position, and local Z angle
in degrees relative to the upright rest axes. Missing bones mean zero delta at
that pose; they do **not** inherit a previous key. Include the bone explicitly
at multiple keys to hold an offset. Absolute world positions are never keyed.

The builder converts pixels via PPU and keys the entire 21-bone property set,
including zero/rest values for unused bones, neutral XYZ scale, and gaitPhase=0.
This resets walking and previous-action poses consistently. It uses Transform
animation, not discrete sprite frames. Source sprites and equipment labels remain
under SpriteResolver control.

The starter builders use upright/unit bone axes. For an established rotated
bind/rest convention, adapt local deltas/bind axes or author clips manually.
Do not reconstruct a working rig merely to satisfy a starter-tool assumption.

## Directional choreography

Author each track for its actual projection. SOUTH uses depth-based reach/steps;
NORTH reverses forward/near meaning; side views reveal horizontal reach; oblique
views combine both. Consult the eight direction guides for visibility and near/far
limbs. Never mirror a SOUTH action or move equipment from Hand_R to Hand_L to
make an occluded weapon visible. Two-handed actions move the real second hand
toward the shaft/grip of the weapon attached to its owning hand.

Walk's counter-swing and 0.5-second timing apply to walking only. A shield block,
bow draw, head nod, spell gesture, or fall has its own coordination. Head motion
can be expressive when that is the action's intent; compensate inherited bob
only where comparative stability is wanted.

Actions can animate Visual/Root for visual whole-body offsets such as crouch,
hop, or collapse. Keep the character's actual world root and collisions separate.
Do not silently move the collision capsule or apply damage from a visual pose.

## Depth and endpoints

Each pose can override complete-limb orders and selected renderer orders. The
runtime uses the latest passed pose for discrete sorting while bones interpolate.
An override lasts only until the next pose: omitting it at the next key restores
that direction's baseline, so restate it to hold it. Without limb override, the
action uses the direction's idleDepth, not a walking order from the previous step.

For a loop, first/last bone offsets and depth must match. Full 360-degree rotations
can have equivalent endpoint angles; verify their continuous loop visually.
For a returning one-shot, normally author a neutral final recovery pose so the
zero-blend return to idle/walk has no pop. A held death/sit pose may intentionally
end away from rest. Validate overlap through the whole rotation range; extreme
joint gaps need source-art repair or appropriate authored pieces.

## Build and inspect

1. Finish the shared character/profile and capture requested directional rests.
2. Create the action asset through Unity plugin-driven editor APIs. Codex
   should populate the asset/poses concretely through the connected plugin,
   rather than returning a choreography proposal alone.
3. Use CharacterActionAnimationBuilder with the selected scene root, authored
   asset, actual PPU, and an Actions output folder. It adds states to the existing
   standard controller and refuses existing files/states; it does not replace the
   controller or regenerate the sixteen locomotion states.
4. Inspect Action_<ActionId>_<Direction> clips and scrubbing at every key/marker.
5. Assign the test action on EightDirectionCharacter and use Play Test Action
   during play, or call TryPlayAction from gameplay. Test cancel/end and return.
6. Verify every requested direction with relevant equipment and asymmetric limb
   markings, then return exact assets and tested status.

New state names are protected from collisions. To revise a generated action,
edit its existing clips deliberately or remove only its generated states/files
explicitly before rebuilding. A different output folder alone cannot bypass an
existing state collision. Keep the definition, curves, and marker times in sync;
do not edit the definition at runtime without regenerating compatible clips.

## Acceptance

- Motion fulfills the requested action rather than repeating a walk cycle.
- Every requested direction has an authored track and generated state/clip.
- Entry pose, anticipation, movement, cue/contact, and exit match the specification.
- Equipment remains attached to the same physical hand throughout.
- No joint gaps, sprite-frame substitution, heavy deformation, or identity swaps.
- Loop endpoints/depth are seamless; one-shot return is deliberate; held pose stays.
- Cancellation restores current idle/walk and base depth when allowed.
- Locomotion does not overwrite the action on the next Update.
- Markers fire with correct names/times; repeat appropriately on loops; stopped
  actions do not continue publishing obsolete cues.
- Visual world-offset effects remain distinct from gameplay movement/collisions.
- Unity plugin calls, compile results, generated asset checks, and visual play-mode
  results are reported separately. If plugin readiness cannot be established,
  report a blocked invocation and setup step; do not create an offline replacement.
