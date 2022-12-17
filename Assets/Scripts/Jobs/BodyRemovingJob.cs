using Unity.Entities;

namespace GravityAttraction
{
    public partial struct BodyRemovingJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter CommandBuffer;

        private void Execute([EntityIndexInQuery] int index, in Entity entity, in Mass mass)
        {
            if (mass.Value <= 0)
            {
                CommandBuffer.SetComponentEnabled<Mass>(index, entity, false);
            }
        }
    }
}