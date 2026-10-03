# Migrate v1 source rigs to the v2 hybrid default

v2.0.0 keeps all 13 skill names and the plugin ID `unity-character-creation-plugin`. It changes default gameplay from live modular bone animation to rig-assisted production and cleaned layered frame playback. No automatic asset conversion is performed by installing/updating the plugin.

1. Preserve existing canonical character identity, neutral direction art, source pieces, rigs, source AnimationClips and item IDs. Inspect quality/revisions instead of regenerating the master. Add missing FRAME_BANK_SPEC.md and new spec fields without replacing populated files.
2. Record `pipeline_mode: hybrid-baked-frames`, accepted user decision and revision. Earlier blanket "no frame animation" policies are superseded for this requested migration. Do not silently change an unrelated project's explicit pipeline decision; reconcile the requested mode in its specs first.
3. Keep source rigs/clips in Authoring/ and isolate a reproducible bake scene/camera. Fix known source joint/oblique defects before exporting. Separate outfit/body/hair/equipment coverage and assign render passes/occluders.
4. Start with South Idle/Walk and one outfit/weapon. Bake, clean, import and validate these frame banks, then create the runtime pass-renderer prefab and shared clock. Real source grip sockets remain useful for item baking; live runtime bone parenting is replaced by item banks/per-frame anchors.
5. Migrate action/state ownership and markers deliberately. Preserve movement/input/physics and ground-sorting contracts that already work. Replace only the old visual bone/sprite selection system when the new runtime passes QA; avoid two concurrent animation owners.
6. Expand requested SW/other directions/actions/items after the baseline, reusing semantic timing and source clips. Mark old runtime visual/coverage checks stale; keep source checks only if their inputs/revisions remain valid. Retest missing-part incidents in source, final images and runtime independently.

Sprites/frames per item are a production cost. Estimate coverage before promising a large wardrobe or many actions. Archive raw exports and final pixel cleanup to keep re-baking reproducible. Static plugin validation never substitutes for actual bake files and rendered gameplay evidence.
