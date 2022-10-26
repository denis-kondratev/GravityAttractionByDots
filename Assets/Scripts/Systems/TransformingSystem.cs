using Unity.Burst;
using Unity.Entities;

namespace GravityAttraction
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(AttractingSystem))]
    [BurstCompile]
    public partial struct TransformingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            
        }

        public void OnDestroy(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new TransformingJob
            {
                DeltaTime = SystemAPI.Time.fixedDeltaTime,
                MassToSizeFactor = SystemAPI.GetSingleton<WorldConfig>().MassToSizeFactor
            };

            job.ScheduleParallel();
        }
    }
}