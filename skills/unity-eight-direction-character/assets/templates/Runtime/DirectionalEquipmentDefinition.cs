using System;
using UnityEngine;

namespace RpgEightDirection
{
    [Serializable]
    public sealed class DirectionItemAppearance
    {
        public Direction8 direction;
        public SpriteChoice[] sprites = new SpriteChoice[0];
        // Complete local pose relative to the socket's parent hand, not a delta.
        public SocketPose weaponSocket = SocketPose.Identity;
    }

    [CreateAssetMenu(menuName = "Eight Direction Character/Equipment")]
    public sealed class DirectionalEquipmentDefinition : ScriptableObject
    {
        public string itemId;
        public string displayName;
        public EquipmentSlot slot;
        public DirectionItemAppearance[] directions = CreateDirections();

        public bool TryGet(Direction8 direction, out DirectionItemAppearance appearance)
        {
            appearance = null;
            if (directions == null || directions.Length != 8) return false;
            var seen = new bool[8];
            foreach (var entry in directions)
            {
                if (entry == null || (int)entry.direction < 0 || (int)entry.direction >= 8 ||
                    seen[(int)entry.direction]) return false;
                seen[(int)entry.direction] = true;
                if (entry.direction == direction) appearance = entry;
            }
            return appearance != null;
        }

        private static DirectionItemAppearance[] CreateDirections()
        {
            var result = new DirectionItemAppearance[8];
            for (int i = 0; i < result.Length; i++)
                result[i] = new DirectionItemAppearance { direction = (Direction8)i };
            return result;
        }
    }
}
