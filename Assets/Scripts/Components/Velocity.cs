using Unity.Entities;
using Unity.Mathematics;

namespace GravityAttraction
{
    public struct Velocity : IComponentData
    {
        public float3 Value;
    }
}