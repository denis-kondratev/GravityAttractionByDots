using Unity.Burst;
using Unity.Entities;

namespace GravityAttraction
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(CollisionHandlingSystem))]
    public partial struct BodyDisablingSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }
        
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var cbs = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var commandBuffer = cbs.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            var job = new BodyDisablingJob
            {
                CommandBuffer = commandBuffer
            };

            job.ScheduleParallel();
        }
    }
}