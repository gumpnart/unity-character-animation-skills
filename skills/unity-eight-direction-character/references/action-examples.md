# Example specifications

These demonstrate the request-to-specification step. They are authoring examples,
not finished assets or a restricted action catalog. Bone poses must be authored
for the character's proportions and each projection.

## MineOre

All eight directions, one-shot, interruptible, equipment locked, return to current
idle/walk. Pickaxe_R follows Hand_R; Hand_L supports the shaft. Duration 0.9s.

| Seconds | Pose | Cue |
|---|---|---|
| 0.00 | settled two-handed grip | ActionStarted callback |
| 0.20 | lift tool, small torso anticipation | — |
| 0.34 | begin downward strike, elbows lead | — |
| 0.45 | contact pose, small body compression | Impact marker |
| 0.60 | follow-through with readable wrists | — |
| 0.90 | neutral recovery | ActionCompleted callback |

Author Shoulder/UpperArm/Forearm/Hand poses for both physical arms, with subtle
Chest/Pelvis support and stable feet. Tool offsets remain item grip calibration;
the skeleton makes the swing. SOUTH projects the contact toward/below the camera,
NORTH away/above, side views show the swing arc, and diagonals need actual oblique
poses. Depth switches before impact if the tool crosses the torso.

## CelebrationLoop

All eight directions, looping, interruptible. Duration 1.6s; expressive shoulders
and head are intended. Return is explicit via TryEndAction.

| Seconds | Pose | Cue |
|---|---|---|
| 0.00 | neutral celebratory stance | ActionStarted callback |
| 0.32 | real L leg takes weight, both hands rise | — |
| 0.64 | small upward body lift | Clap marker |
| 0.96 | real R leg takes weight, shoulders sway | — |
| 1.28 | second lift | Clap marker |
| 1.60 | exact starting pose and depth | loop repeats |

The asymmetric weight shift moves actual bones, not mirrored art. Marker names
may repeat at distinct times. End loop at a deliberate gameplay moment; current
template release is immediate, not a queued cycle-boundary exit.

## CollapseHold

South and North initially, one-shot, noninterruptible, equipment locked. Duration
1.1s, completion=HoldLastPose. Directions are explicitly limited for this example.

| Seconds | Pose | Cue |
|---|---|---|
| 0.00 | current direction's neutral stance | ActionStarted callback |
| 0.25 | knees bend, torso tilts | — |
| 0.62 | visual root/pelvis lower; head follows intent | Fall marker |
| 1.10 | stable terminal collapsed pose | ActionCompleted callback |

Terminal pose persists until TryEndAction, which restores a valid idle/walk.
This creates the visual action only; health, ragdoll, collision, respawn, and death
state logic belong to the requested gameplay system.
