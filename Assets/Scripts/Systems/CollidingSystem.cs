using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace GravityAttraction
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(AttractingSystem))]
    public partial struct CollidingSystem : ISystem
    {
        private EntityQuery _query;
        private Mass _m0;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _query = new EntityQueryBuilder(Allocator.TempJob)
                .WithAll<LocalToWorldTransform, Velocity>()
                .WithAllRW<Mass>()
                .Build(ref state);
            
            _m0 = new Mass { Value = 0 };
        }

        public void OnDestroy(ref SystemState state)
        {

        }
        
        public void OnUpdate(ref SystemState state)
        {
            var transforms = _query.ToComponentDataArray<LocalToWorldTransform>(Allocator.TempJob);
            var collisions = new NativeList<int2>(transforms.Length, Allocator.TempJob);

            var collisionDetectingJob = new CollisionDetectingJob
            {
                EntityCount = transforms.Length,
                Transforms = transforms,
                Collisions = collisions.AsParallelWriter(),
                CollisionDistanceFactor = SystemAPI.GetSingleton<WorldConfig>().CollisionDistanceFactor
            };

            var jobHandle = collisionDetectingJob.Schedule(transforms.Length, 64);
            
            jobHandle.Complete();

            if (collisions.Length == 0)
            {
                transforms.Dispose();
                collisions.Dispose();
                return;
            }
            
            var entities = _query.ToEntityArray(Allocator.Temp);

            for (var i = collisions.Length - 1; i >= 0; i--)
            {
                var collision = collisions[i];
                var entity1 = entities[collision.x];
                var entity2 = entities[collision.y];
                
                var m1 = SystemAPI.GetComponent<Mass>(entity1).Value;
                
                if (m1 <= 0) continue;
                
                var m2 = SystemAPI.GetComponent<Mass>(entity2).Value;
                
                if (m2 <= 0) continue;

                var uniformScaleTransform = transforms[collision.x].Value;
                var v1 = SystemAPI.GetComponent<Velocity>(entity1).Value;
                var v2 = SystemAPI.GetComponent<Velocity>(entity2).Value;
                var p1 = uniformScaleTransform.Position;
                var p2 = transforms[collision.y].Value.Position;
                var mass = m1 + m2;
                var velocity = (v1 * m1 + v2 * m2) / mass;
                uniformScaleTransform.Position = (p1 * m1 + p2 * m2) / mass;
                
                SystemAPI.SetComponent(entity2, _m0);
                SystemAPI.SetComponent(entity1, new Mass { Value = mass });
                SystemAPI.SetComponent(entity1, new Velocity { Value = velocity});
                SystemAPI.SetComponent(entity1, new LocalToWorldTransform { Value = uniformScaleTransform});
            }

            transforms.Dispose();
            collisions.Dispose();
        }
    }
}