using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace GravityAttraction
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct AttractingSystem : ISystem
    {
        private EntityQuery _query;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _query = new EntityQueryBuilder(Allocator.TempJob)
                .WithAll<LocalTransform, Mass>()
                .Build(ref state);
        }

        public void OnDestroy(ref SystemState state) { }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var worldConfig = SystemAPI.GetSingleton<WorldConfig>();
            var masses = _query.ToComponentDataArray<Mass>(Allocator.TempJob);
            var transforms = _query.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
            var entities = _query.ToEntityArray(Allocator.TempJob);
            
            var job = new AttractingJob
            {
                BodyCount = _query.CalculateEntityCount(),
                Masses = masses,
                Transforms = transforms,
                Entities = entities,
                DeltaTime = SystemAPI.Time.fixedDeltaTime,
                MinDistance = worldConfig.MinDistanceToAttract,
                GravitationalConstant = worldConfig.GravitationalConstant,
                CollisionFactor = worldConfig.CollisionFactor
            };

            job.ScheduleParallel();
            state.Dependency.Complete();
            masses.Dispose();
            transforms.Dispose();
        }
    }
}