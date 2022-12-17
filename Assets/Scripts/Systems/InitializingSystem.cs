using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GravityAttraction
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial struct InitializingSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }

        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            try
            {
                InitializeWorld(ref state);
            }
            finally
            {
                state.Enabled = false;
            }
        }

        [BurstCompile]
        private void InitializeWorld(ref SystemState state)
        {
            var worldConfig = SystemAPI.GetSingleton<WorldConfig>();
            var bodyPrefab = SystemAPI.GetSingleton<BodyPrefab>();
            state.EntityManager.Instantiate(bodyPrefab.Value, worldConfig.BodyCount, Allocator.Temp);
            var sqrtWorldRadius = math.pow(worldConfig.WorldRadius, 1f/3f);

            var job = new InitializingJob
            {
                WorldConfig = worldConfig,
                SqrtWorldRadius = sqrtWorldRadius,
                MassToScaleRatio = worldConfig.MassToScaleRatio
            };

            job.ScheduleParallel();
        }
    }
}