using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace GravityAttraction
{
    public partial struct TransformingJob : IJobEntity
    {
        public float DeltaTime;
        public float MassToSizeFactor;
        
        public void Execute(
            ref LocalTransform transform, 
            in Velocity velocity, 
            in Mass mass)
        {
            transform.Position += velocity.Value * DeltaTime;
            transform.Scale = math.pow(mass.Value, 1f/3f) * MassToSizeFactor;
        }
    }
}