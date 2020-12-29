using UnityEngine;

namespace common
{
    public static class Vector3Extension
    {
        public static Vector3 WithOffset(this Vector3 single, float xOffset)
        {
            return new Vector3
            {
                x = single.x + xOffset,
                y = single.y,
                z = single.z
            };
        }
    }
}