namespace RpgEightDirection
{
    public static class CharacterRigPaths
    {
        public const string Root = "Visual/Root";
        public const string Pelvis = Root + "/Pelvis";
        public const string SpineLower = Pelvis + "/SpineLower";
        public const string SpineUpper = SpineLower + "/SpineUpper";
        public const string Chest = SpineUpper + "/Chest";
        public const string Neck = Chest + "/Neck";
        public const string Head = Neck + "/Head";
        public const string ShoulderL = Chest + "/Shoulder_L";
        public const string ShoulderR = Chest + "/Shoulder_R";
        public const string ArmL = ShoulderL + "/UpperArm_L";
        public const string ArmR = ShoulderR + "/UpperArm_R";
        public const string ForearmL = ArmL + "/Forearm_L";
        public const string ForearmR = ArmR + "/Forearm_R";
        public const string HandL = ForearmL + "/Hand_L";
        public const string HandR = ForearmR + "/Hand_R";
        public const string ThighL = Pelvis + "/Thigh_L";
        public const string ThighR = Pelvis + "/Thigh_R";
        public const string ShinL = ThighL + "/Shin_L";
        public const string ShinR = ThighR + "/Shin_R";
        public const string FootL = ShinL + "/Foot_L";
        public const string FootR = ShinR + "/Foot_R";
        public static readonly string[] All =
        {
            Root, Pelvis, SpineLower, SpineUpper, Chest, Neck, Head,
            ShoulderL, ArmL, ForearmL, HandL, ShoulderR, ArmR, ForearmR, HandR,
            ThighL, ShinL, FootL, ThighR, ShinR, FootR
        };
        public static readonly string[] SpriteCategories =
        {
            "Hair_Back", "Head", "Hair_Front", "Torso_Upper", "Torso_Lower", "Pelvis",
            "UpperArm_L", "Forearm_L", "Hand_L", "UpperArm_R", "Forearm_R", "Hand_R",
            "Thigh_L", "Shin_L", "Foot_L", "Thigh_R", "Shin_R", "Foot_R",
            "Weapon_R", "Weapon_L", "HeadEquipment", "ChestEquipment", "BackEquipment", "WaistEquipment"
        };
    }
}
