using Unity.Mathematics;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public static class MathHelpers
    {
        /// <summary>
        /// math.PI/2
        /// </summary>
        public const float HalfPI = 1.57079637f;
        
        /// <summary>
        /// 2 * math.PI
        /// </summary>
        public const float PI2 = 6.28318531f;

        public static float GetHeading(float3 objectPosition, float3 targetPosition)
        {
            var x = objectPosition.x - targetPosition.x;
            var y = objectPosition.y - targetPosition.y;
            return math.atan2(y, x) + HalfPI;
        }
    }
}
