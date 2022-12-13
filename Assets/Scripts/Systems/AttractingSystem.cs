using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace GravityAttraction
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct AttractingSystem : ISystem
    {
        private EntityQuery _query;
        
        public void OnCreate(ref SystemState state)
        {
            _query = new EntityQueryBuilder(Allocator.TempJob)
                .WithAll<LocalTransform, Mass>()
                .Build(ref state);
        }

        public void OnDestroy(ref SystemState state)
        {
            
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var worldConfig = SystemAPI.GetSingleton<WorldConfig>();

            var job = new AttractingJob
            {
                BodyCount = _query.CalculateEntityCount(),
                Masses = _query.ToComponentDataArray<Mass>(Allocator.TempJob),
                Transforms = _query.ToComponentDataArray<LocalTransform>(Allocator.TempJob),
                FixedDeltaTime = SystemAPI.Time.fixedDeltaTime,
                MinDistance = worldConfig.MinDistanceToAttract,
                GravitationalConstant = worldConfig.GravitationalConstant
            };

            job.ScheduleParallel();
        }
    }
}