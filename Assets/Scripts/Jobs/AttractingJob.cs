using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace GravityAttraction
{
    [BurstCompile]
    public partial struct AttractingJob : IJobEntity
    {
        [ReadOnly] public int BodyCount;
        [ReadOnly] public NativeArray<Mass> Masses;
        [ReadOnly] public NativeArray<LocalTransform> Transforms;
        [ReadOnly] public NativeArray<Entity> Entities;
        [ReadOnly] public float DeltaTime;
        [ReadOnly] public float MinDistance;
        [ReadOnly] public float GravitationalConstant;
        [ReadOnly] public float CollisionFactor;
        
        [BurstCompile]
        private void Execute(
            [EntityIndexInQuery] int index,
            ref PhysicsVelocity velocity,
            ref Collision collision,
            in Mass mass)
        { 
            if (mass.Value <= 0)
            {
                return;
            }
            
            var force = float3.zero;
            var position = Transforms[index].Position;
            collision = new Collision();
            var hasCollision = false;

            for (var i = 0; i < BodyCount; i++)
            {
                if (index == i || Masses[i].Value <= 0)
                {
                    continue;
                }
                
                var distance = math.distance(position, Transforms[i].Position);
                var permittedDistance = math.max(distance, MinDistance);
                
                force += GravitationalConstant * mass.Value * Masses[i].Value * (Transforms[i].Position - position) 
                         / (permittedDistance * permittedDistance * permittedDistance);

                if (hasCollision || i < index || !HasCollision(distance, index, i))
                {
                    continue;
                }

                hasCollision = true;
                collision.Entity = Entities[i];
            }

            velocity.Linear += DeltaTime / mass.Value * force;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HasCollision(float distance, int i, int j)
        {
            var leftScale = Transforms[i].Scale;
            var rightScale = Transforms[j].Scale;
            
            if (distance < (leftScale + rightScale) * CollisionFactor)
            {
                return true;
            }
            
            var (maj, min) = leftScale > rightScale ? (leftScale, rightScale) : (rightScale, leftScale);
            return distance < maj / 2 - min;
        }
    }
}