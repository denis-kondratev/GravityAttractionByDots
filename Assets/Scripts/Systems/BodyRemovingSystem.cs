using Unity.Burst;
using Unity.Entities;

namespace GravityAttraction
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderLast = true)]
    public partial struct BodyRemovingSystem : ISystem
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
            var cbs = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var commandBuffer = cbs.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            var job = new BodyRemovingJob
            {
                Buffer = commandBuffer
            };

            job.ScheduleParallel();
        }
    }
}