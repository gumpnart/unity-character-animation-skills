# Stage routing and dependency gates

| Stage | Prerequisites | Durable outputs / next checks |
| --- | --- | --- |
| master-character | brief or existing base art | master + CHARACTER_SPEC → direction-master |
| direction-master | validated master | neutral view(s) + DIRECTION_SPEC → rig-ready-parts |
| rig-ready-parts | validated requested neutral view | aligned overlapping pieces → unity-asset-import |
| unity-asset-import | part inventory | imported assets + policy → unity-skeleton-rig |
| unity-skeleton-rig | aligned imported assets | shared rig/prefab/sockets → joint-skinning-validation |
| joint-skinning-validation | actual rig | joint range evidence → clip/equipment stages |
| animation-clip-authoring | calibrated direction, validated joints | any named action + ANIMATION_SPEC |
| directional-animation-system | validated base action, requested views/calibration | 4/8 direction mappings and action variants |
| modular-equipment-system | shared rig/sockets, item artwork | EQUIPMENT_SPEC + swaps + test weapon |
| animator-controller | validated clips and action/parameter contract | saved controller + transition evidence |
| runtime-character-controller | prefab, controller contract, direction/item data | movement/facing/actions/sorting/test scene |
| character-production-qa | scoped artifacts + runtime | coverage, regressions, evidence, stage routing |
| character-production-orchestrator | repository/brief | initialize specs, select/resume stage, track dependencies |

Stages form a dependency graph, not an excuse to skip checks. Equipment can be developed after joint validation alongside initial clips. Directional expansion needs a tested base action and the neutral/parts/import/rig gates for each additional direction. Runtime does not require nonexistent unrequested combat actions.

For a new character, first prove master → South neutral → parts/import → rig → joints → IdleSouth → WalkSouth → equipment swap → attached test weapon → controller/runtime/QA. Then expand requested direction/action coverage, revisiting joints when actions exceed validated ranges. A user may request all eight, but South is still the first small validation checkpoint.

If existing assets satisfy a prerequisite, inspect and record their validation and revision; do not recreate them blindly. If art changes, invalidate affected parts/import/rig calibration/clips/equipment/QA. If rig binding paths change, invalidate clip bindings and controller/runtime checks. If action timing changes, retest event markers and gameplay timing. If only a weapon sprite changes, retest grip/sorting/coverage without recreating Walk clips.

For direct skill use, a missing prerequisite routes back to its owner rather than fabricating placeholders. Save the blocker, output inventory and concrete next task in the canonical stage ledger. Do not require user confirmation for ordinary implementation unless their task explicitly calls for approval.
