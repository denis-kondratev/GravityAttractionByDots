using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GravityAttraction
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial struct WorldCreatingSystem : ISystem
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
            var worldConfig = SystemAPI.GetSingleton<WorldConfig>();
            state.EntityManager.Instantiate(worldConfig.BodyPrefab, worldConfig.BodyCount, Allocator.Temp);
            var random = new Random();
            random.InitState(1);
            var sqrtWorldRadius = math.pow(worldConfig.WorldRadius, 1f/3f);

            var job = new BodyInitializingJob
            {
                Random = random,
                WorldConfig = worldConfig,
                SqrtWorldRadius = sqrtWorldRadius
            };

            job.Run();
            
            state.Enabled = false;
        }
    }
}