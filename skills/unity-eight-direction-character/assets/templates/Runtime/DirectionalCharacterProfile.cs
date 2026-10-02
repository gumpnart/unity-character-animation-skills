using System;
using UnityEngine;

namespace RpgEightDirection
{
    public enum EquipmentSlot { Weapon_R, Weapon_L, Head, Chest, Back, Waist }

    [Serializable]
    public struct SpriteChoice
    {
        public string category;
        public string label;
    }

    [Serializable]
    public struct RendererOrder
    {
        public string category;
        public int order;
    }

    [Serializable]
    public struct LimbOrders
    {
        public int legL, legR, armL, armR;
        public LimbOrders(int leftLeg, int rightLeg, int leftArm, int rightArm)
        {
            legL = leftLeg; legR = rightLeg; armL = leftArm; armR = rightArm;
        }
    }

    [Serializable]
    public struct SocketPose
    {
        public Vector3 position;
        public float rotationZ;
        public float scale;
        public static SocketPose Identity { get { return new SocketPose { scale = 1f }; } }

        public bool IsValid()
        {
            return Direction8Utility.IsFinite(position.x) && Direction8Utility.IsFinite(position.y) &&
                Direction8Utility.IsFinite(position.z) && Direction8Utility.IsFinite(rotationZ) &&
                Direction8Utility.IsFinite(scale) && scale > 0f;
        }

        public void Apply(Transform socket)
        {
            socket.localPosition = position;
            socket.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
            socket.localScale = Vector3.one * scale;
        }
    }

    [Serializable]
    public struct BoneRestPose
    {
        public string path;
        public Vector3 position;
    }

    [Serializable]
    public sealed class DirectionPose
    {
        public Direction8 direction;
        public SpriteChoice[] defaults = new SpriteChoice[0];
        public RendererOrder[] rendererOrders = new RendererOrder[0];
        public BoneRestPose[] bones = new BoneRestPose[0];
        public LimbOrders idleDepth;
        // Contact A, Passing A, Contact B, Passing B. Contact A repeats at loop end.
        public LimbOrders[] walkDepth = new LimbOrders[4];
        public SocketPose weaponR = SocketPose.Identity;
        public SocketPose weaponL = SocketPose.Identity;
        [Header("Starter motion: tune against each directional artwork")]
        public Vector2 stridePixels;
        [Min(0f)] public float bodyBobPixels = 1f;
        [Min(0f)] public float idleBreathPixels = 0.25f;
        [Range(0f, 0.5f)] public float armSwingRatio = 0.3f;
        [Min(0f)] public float footLiftPixels = 0.5f;
        public float leftKneeBend = 3f;
        public float rightKneeBend = -3f;
    }

    [CreateAssetMenu(menuName = "Eight Direction Character/Profile")]
    public sealed class DirectionalCharacterProfile : ScriptableObject
    {
        public DirectionPose[] directions = CreateDirections();

        public bool TryGet(Direction8 direction, out DirectionPose pose)
        {
            pose = null;
            if (directions == null || directions.Length != 8) return false;
            var seen = new bool[8];
            foreach (var entry in directions)
            {
                if (entry == null || (int)entry.direction < 0 || (int)entry.direction >= 8 ||
                    seen[(int)entry.direction]) return false;
                seen[(int)entry.direction] = true;
                if (entry.direction == direction) pose = entry;
            }
            return pose != null;
        }

        private static DirectionPose[] CreateDirections()
        {
            var result = new DirectionPose[8];
            for (int i = 0; i < result.Length; i++)
            {
                Direction8 direction = (Direction8)i;
                bool north = direction == Direction8.North;
                bool south = direction == Direction8.South;
                bool leftNear = direction == Direction8.West || direction == Direction8.SouthWest ||
                    direction == Direction8.NorthWest;
                var contactA = new LimbOrders(5, -5, -10, 10);
                var contactB = new LimbOrders(-5, 5, 10, -10);
                var fixedNear = leftNear ? new LimbOrders(5, -5, 10, -10) :
                    new LimbOrders(-5, 5, -10, 10);
                if (north) { var swap = contactA; contactA = contactB; contactB = swap; }
                var walk = south || north
                    ? new[] { contactA, contactB, contactB, contactA }
                    : new[] { fixedNear, fixedNear, fixedNear, fixedNear };
                result[i] = new DirectionPose
                {
                    direction = direction,
                    stridePixels = Direction8Utility.Forward(direction) * 2f,
                    idleDepth = south || north ? contactA : fixedNear,
                    walkDepth = walk
                };
            }
            return result;
        }
    }
}
