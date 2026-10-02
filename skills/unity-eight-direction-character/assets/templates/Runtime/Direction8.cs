using UnityEngine;

namespace RpgEightDirection
{
    // Stable serialized values, in the order requested by the handoff.
    public enum Direction8
    {
        South, SouthWest, West, NorthWest, North, NorthEast, East, SouthEast
    }

    public static class Direction8Utility
    {
        private static readonly Direction8[] AngularOrder =
        {
            Direction8.East, Direction8.NorthEast, Direction8.North, Direction8.NorthWest,
            Direction8.West, Direction8.SouthWest, Direction8.South, Direction8.SouthEast
        };

        public static bool IsFinite(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }

        public static bool IsFinite(Vector2 value)
        {
            return IsFinite(value.x) && IsFinite(value.y);
        }

        public static float Angle(Direction8 direction)
        {
            for (int i = 0; i < AngularOrder.Length; i++)
                if (AngularOrder[i] == direction) return i * 45f;
            return 270f;
        }

        public static Vector2 Forward(Direction8 direction)
        {
            float angle = Angle(direction) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        public static Direction8 Select(Vector2 screenVelocity, Direction8 current,
            float deadZone = 0.01f, float hysteresisDegrees = 5f)
        {
            if (!IsFinite(screenVelocity) ||
                screenVelocity.magnitude <= Mathf.Max(0f, deadZone)) return current;
            float angle = Mathf.Atan2(screenVelocity.y, screenVelocity.x) * Mathf.Rad2Deg;
            float margin = Mathf.Clamp(hysteresisDegrees, 0f, 15f);
            if (Mathf.Abs(Mathf.DeltaAngle(Angle(current), angle)) < 22.5f + margin)
                return current;
            int sector = Mathf.FloorToInt(Mathf.Repeat(angle + 22.5f, 360f) / 45f);
            return AngularOrder[sector];
        }
    }
}
