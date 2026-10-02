# Setup and verification

## Setup

Start every invocation with the mandatory [Unity plugin call and editor
readiness check](unity-plugin-integration.md). Use the connected plugin for the
editor operations below. If readiness cannot be established, report the blocker
and stop implementation; these instructions do not authorize an offline fallback.

1. Inspect Unity and installed package versions. Add supported 2D Animation and
   PSD Importer versions if needed. Import layered PSB/PSD artwork exposing the
   named parts; use PSB if the installed importer requires it. Point filtering,
   uniform PPU, no lossy compression, and source overlap are mandatory checks.
2. Assemble Character8's exact skeleton, rigid resolver children, shared library,
   top SortingGroup, four nested limb groups, and six sockets from architecture.md.
   Create all eight sets of default body/outfit/empty socket labels.
3. Copy all Runtime templates into Scripts/Runtime and the editor template into
   Scripts/Editor. Keep names matching the public Unity component/asset class.
   Compile in Unity before creating profile/item assets. Namespaced C# types are
   `RpgEightDirection.*`; include that namespace in gameplay code.
4. Create a profile via Create → Eight Direction Character → Profile. It starts
   with eight unique directions, starter stride vectors, and four depth entries
   per direction. Default sprites, fixed orders, and bones still require authoring.
5. Add EightDirectionCharacter to the prefab root beside Animator. Assign:
   animator; shared library; profile; physical L/R thigh and upper-arm groups;
   direct Hand_R/Weapon_R and Hand_L/Weapon_L socket transforms.
6. Add one resolver binding per category. The runtime template requires all 18 body parts
   plus six sockets. Bind Torso_Upper/Lower as equipment-controlled Chest, weapons
   to their matching slots, HeadEquipment to Head, BackEquipment to Back, and
   WaistEquipment to Waist. Keep ChestEquipment uncontrolled/empty if swapping
   torso pieces rather than armor overlays. Ordinary hair/body/limbs are uncontrolled.
7. In edit mode, outside Animation-window preview, author each of the eight rest
   poses. Select its correct resolver labels, source-art alignment, fixed renderer
   orders, idle group depth, and empty weapon grip poses. Keep bones upright/unit
   scale for the starter builder; orient visual children to match the art.
8. Select the scene-instance root. Open Tools → Eight Direction Character →
   Animation Builder, select the direction, and Capture Current Authored Direction.
   Repeat eight times. Capturing reads the current scene; it does not discover
   the right directional artwork, rotate sprites, or make a SOUTH pose correct
   for another direction. Capture the unequipped defaults, not an equipped item.
9. Inspect captured profile entries. Tune per-direction stride, knee bends, foot
   lift, arm amplitude, bob/breathing, idle order, and all four walkDepth entries.
   Fixed renderer orders cover every bound category, including limb internals.
10. Enter actual PPU and a fresh Assets output folder, then Build 16 Clips and
    Assign Controller. Existing assets are never overwritten. The builder requires
    all eight captured bone sets and valid default sprites/orders/sockets. It
    creates starter motion; inspect/tune every generated clip in Unity afterward.
11. Save/apply intended prefab assignments, especially the controller. Create an
    orthographic test scene with a contrasting background and a prefab instance.
    Use Inspector test input to try screen vectors for all eight directions;
    set it to zero to inspect the last direction's idle pose.
12. Create two directional chest items and two right-hand weapons via Create →
    Eight Direction Character → Equipment. Supply all eight unique variants.
    Chest variants choose both torso categories; weapon variants choose Weapon_R
    and a full positive socket-local pose. Start grip scale at 1. Test swapping
    with the component's Equip Test Item context menu during play.

## Gameplay example

```csharp
using RpgEightDirection;

// During the caller's Update (before the driver's execution order 100):
character.SetScreenVelocity(cameraProjectedVelocity);
// The caller independently moves the world root using the game's movement logic.

if (!character.TryEquip(leatherArmor, out string error))
    Debug.LogError(error);
character.TryEquip(swordRight, out error);
character.TryUnequip(EquipmentSlot.Weapon_R, out error);

// Run from setup tooling to check an item's actual eight appearances.
for (int i = 0; i < 8; i++)
    if (!character.TryValidateDirection((Direction8)i, out error, swordRight))
        Debug.LogError(error);
```

For a tilted orthographic camera, project world velocity onto camera right/up.
The driver receives that result. It does not implement collision, input bindings,
or transform movement. Do not normalize the velocity before a magnitude-based
dead-zone check if preserving true near-zero motion is important.

## Per-direction matrix

Record Unity/package versions and compile result. For **each** of the eight
directions, check both idle and walk, then repeat with both armors and both weapons:

- Idle rest stance is correct for the view, with steady head and readable limbs.
- 0.000 Contact A: physical L leg and R arm advance along movement.
- 0.125 Passing A: L moves back, R forward, body rises one source-art pixel.
- 0.250 Contact B: physical R leg and L arm advance.
- 0.375 Passing B: R moves back, L forward, second body rise.
- 0.500 equals 0.000, with no pop over at least ten loops.
- Depth agrees with the view; complete groups and attached weapons sort correctly.
- All shoulder/elbow/wrist/hip/knee/ankle gaps remain covered.
- Crisp silhouettes, no heavy deformation, no negative scales/mirroring.
- Chest swaps update every owned piece without changing bones or restarting gait.
- Weapon grip follows the same physical Hand_R in every pose and view.
- Unequip restores this direction's defaults and socket poses.

## Turning and failure checks

- Turn through all adjacent pairs and directly to each opposite view while idle
  and while walking. Turn at 0.0, 0.125, 0.25, and 0.375 gait times; fractional
  phase and physical limb identity must survive Walk → Walk.
- Release input in every sector; last facing remains and the matching idle plays.
- Oscillate around 22.5° sector boundaries inside the hysteresis margin; facing
  remains stable. Cross the margin and confirm one intentional change.
- Supply zero, near-zero, and nonfinite input; direction stays valid.
- Remove an item label in one test direction. Try that turn: report missing
  direction/category/label and keep the previous complete visual appearance.
- Try an item missing a torso choice, duplicate direction, invalid socket scale,
  or unassigned hand group; validate before any visible mutation.
- Try a missing target Animator state. The turn must not apply sprites or sockets.
- Verify no remaining SOUTH driver writes animation, labels, or renderer order.

The driver does not stop world motion on an invalid appearance. Handle that at
the movement layer if the game requires it. Clear/retest missing content before
declaring the eight-direction slice complete.

## Evidence and limitations

Report exact generated asset paths, captured profiles, item IDs, tested directions,
Unity/package versions, compile result, and visual observations. Without a Unity
editor, template inspection and package integrity are possible; compilation,
asset generation, animation evaluation, skin compatibility, and art quality
remain unverified. Under this skill's mandatory plugin policy, an unreachable
plugin/editor blocks implementation. Do not substitute metadata checks or
standalone generated code for the required plugin call and play-mode verification.
