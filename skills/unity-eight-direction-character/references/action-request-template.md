# Request any action

Invoke the skill by name in Codex. Ordinary language is sufficient; this form is
optional and helps when timing or gameplay cues must be exact.

```text
Use $unity-eight-direction-character to create this action animation:

Action: <plain-language name and intent>
Character/project: <prefab or existing character to use>
Directions: all eight [or list a smaller explicit subset]
Duration: <seconds, or let Codex propose a suitable duration>
Playback: one-shot / loop / finish and hold the final pose
Motion: <body posture, limb motion, anticipation, main motion, recovery>
Equipment: <weapon/tool, physical hand, grip constraints, existing item asset>
Markers: <named cues with exact times if known>
Movement: <stationary / caller moves the world root / existing movement behavior>
Cancellation: <allowed or blocked>
Finish: <return to current idle/walk, or hold until explicitly released>
Style: rigid pixel-art pieces; subtle deformation; match the directional camera

Inspect the project, author the skeletal clips and action asset, add Animator
states without replacing locomotion, and verify the requested directions.
```

## Examples

```text
Use $unity-eight-direction-character to create a two-handed mining swing for
all eight directions. Anticipate for 0.2 seconds, strike at 0.45, and recover
by 0.9. Use my Pickaxe_R item on Hand_R, align Hand_L on its shaft, emit an
Impact marker at 0.45, and return to idle/walk. Keep it interruptible.
```

```text
Use $unity-eight-direction-character to create a looping celebration dance
for all eight directions. Let the shoulders and head move expressively, keep
the anatomical legs consistent, and end the loop when gameplay requests it.
Choose a suitable duration and author a seamless endpoint.
```

```text
Use $unity-eight-direction-character to create a death action in South and
North first. Make it a one-shot that holds its final pose, and do not allow
cancellation. Use 1.1 seconds with a gentle collapse; no damage logic is needed.
```

These are examples, not action types baked into the code. For another action,
describe what the character should do. Codex chooses a safe actionId string,
documents any inferred duration/poses, and implements only the requested action.
