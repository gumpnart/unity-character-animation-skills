# Hybrid production stage routing

| Skill | Prerequisite | Durable output / next gate |
| --- | --- | --- |
| master-character | brief/existing design | neutral base outfit + identity/proportions → directions |
| direction-master | validated master | requested neutral views/camera/anchor → parts |
| rig-ready-parts | validated requested views | overlapping editable sources + pass roles → source import |
| unity-asset-import | source parts OR final bake frames | calibrated source imports OR final runtime sprites |
| unity-skeleton-rig | imported source | authoring rig/calibration/sockets → joint checks |
| joint-skinning-validation | source rig OR final frames | source joint evidence OR baked silhouette validation |
| animation-clip-authoring | source rig/joints/action spec | source clip → real bake → cleaned final frames → final import |
| directional-animation-system | validated base action/bank and requested source views | complete requested directional banks/selection |
| modular-equipment-system | source rig/item art/pose schedule | baked item passes, replacements/masks and bank swaps |
| animator-controller | final bank/action contract | one semantic state/action clock and transitions |
| runtime-character-controller | final sprites/banks/controller/items | synchronized frame layers, movement, events, sorting |
| character-production-qa | scoped source/frame/runtime artifacts | evidence and routed fixes |
| character-production-orchestrator | repository/brief/specs | initialize/migrate persistent state and select first dependency |

The graph includes repeated import/validation stages. A source AnimationClip is not a completed runtime animation. The animation-authoring stage owns bake/cleanup registration; final exports return to import/joint QA before gameplay. No extra skill is required to hide these mandatory steps.

New slice: neutral master → South parts/import → authoring rig/joints → Idle/Walk clips → bake/clean/import body/hair/outfit layers → one shared-clock runtime → baked test armor/weapon swap → rendered QA. Then expand requested directions/actions/items. A request for all eight keeps missing combinations pending/blocked rather than reducing scope.

Defect recovery compares authoring pose, raw export, cleaned final frame and runtime composite. Fix only the confirmed owner stage, preserve valid master/source work and mark affected banks/runtime evidence stale. A new item can reuse source action clips but requires compatible frame coverage. Record next work and evidence in the six specs; missing actual tools/assets remain blocked.
