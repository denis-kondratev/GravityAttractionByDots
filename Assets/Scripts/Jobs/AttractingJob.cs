using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace GravityAttraction
{
    [BurstCompile]
    public partial struct AttractingJob : IJobEntity
    {
        [ReadOnly] public int BodyCount;
        [ReadOnly] public NativeArray<Mass> Masses;
        [ReadOnly] public NativeArray<LocalToWorldTransform> Transforms;
        [ReadOnly] public float FixedDeltaTime;
        [ReadOnly] public float MinDistance;
        [ReadOnly] public float GravitationalConstant;

        [BurstCompile]
        public void Execute(
            [EntityInQueryIndex] int index,
            ref Velocity velocity,
            in LocalToWorldTransform transform,
            in Mass mass)
        {
            if (mass.Value <= 0) return;
            
            var force = float3.zero;
            var position = transform.Value.Position;

            for (var i = 0; i < BodyCount; i++)
            {
                if (index == i) continue;

                var distance = math.distance(position, Transforms[i].Value.Position);
                
                if (distance < MinDistance) continue;
                
                force += GravitationalConstant * mass.Value * Masses[i].Value * (Transforms[i].Value.Position - position) 
                         / (distance * distance * distance);
            }
            
            velocity.Value += FixedDeltaTime / mass.Value * force;
        }
    }
}