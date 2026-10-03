# Neutral master character and baseline outfit

Use an existing suitable design if available. A master is a canonical design reference, not the final equipment loadout or a walking screenshot. Inspect actual artwork before recording measurements. Keep age/body style/face/skin/hair identity and handedness consistent with the user brief; do not invent new identity to solve a rig issue.

Proposed baseline: plain fitted sleeveless top, simple fitted shorts, bare feet and one defined hairstyle. Use readable flat/limited colors, consistent outline and lighting, no armor/hat/weapon/bag/cape initially. Existing equivalent modest base clothing can be kept. Clothing is fitted enough to reveal shoulder/hip/knee/ankle landmarks without unnecessarily obscuring joints. Source patches under hidden joins cover intended motion, not a mandatory unclothed body sheet.

South neutral: slightly elevated 3/4 RPG camera, torso upright, weight balanced on both grounded feet, arms slightly separated from torso, hands relaxed and not blocking hips, modest leg separation. Record a common ground anchor/native canvas. Avoid weight-shifted walk poses as the sole neutral calibration. Choose actual resolution/PPU from game camera needs and test at native scale before locking them.

Record measured character height, head ratio, shoulder/pelvis widths, limb lengths, joint landmarks and pixel coordinate convention. These measurements and their source revision are canonical for every direction/action. Do not guess metrics from an unseen or resized image. Aesthetic approval applies when explicitly requested; evidence-based validation is otherwise recorded without repeated approval prompts.

Separate Body identity from BaseClothing and replaceable Hair/Equipment. Define Clothing_Upper and Clothing_Lower replacement independently so equipping torso armor does not remove shorts. Hair_Back/Front supports helmets/head occlusion. Shoes/boots use the same foot/ground anchor. A new visible region requires completed source art; do not let removed clothing expose an undefined patch.

Capture neutral South first, then consistent required direction masters. Save source paths, native dimensions, landmarks, outfit/coverage rules, revision and evidence in CHARACTER_SPEC.md/DIRECTION_SPEC.md. In the hybrid pipeline, source pieces animate on an authoring rig and cleaned images become runtime frames. Final frame retouching preserves identity/proportions and is tracked separately from the raw bake.
