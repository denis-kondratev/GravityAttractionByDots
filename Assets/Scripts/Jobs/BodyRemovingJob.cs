using Unity.Burst;
using Unity.Entities;

namespace GravityAttraction
{
    [BurstCompile]
    public partial struct BodyRemovingJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter CommandBuffer;
        
        [BurstCompile]
        private void Execute([EntityIndexInQuery] int index, in Entity entity, in Mass mass)
        {
            if (mass.Value <= 0)
            {
                CommandBuffer.DestroyEntity(index, entity);
            }
        }
    }
}