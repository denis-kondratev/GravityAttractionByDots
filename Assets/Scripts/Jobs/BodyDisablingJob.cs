using Unity.Burst;
using Unity.Entities;

namespace GravityAttraction
{
    [BurstCompile]
    public partial struct BodyDisablingJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter CommandBuffer;

        [BurstCompile]
        private void Execute([EntityIndexInQuery] int index, in Entity entity, in Mass mass)
        {
            if (mass.Value <= 0)
            {
                CommandBuffer.SetComponentEnabled<Mass>(index, entity, false);
            }
        }
    }
}