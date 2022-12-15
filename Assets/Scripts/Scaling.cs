using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace GravityAttraction
{
    public struct Scaling
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MassToScale(float mass, float massToScaleRatio)
        {
            return math.pow(6 * mass / math.PI, 1/3f) * massToScaleRatio;
        }
    }
}