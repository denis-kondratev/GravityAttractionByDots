using Unity.Burst;
using Unity.Entities;

namespace GravityAttraction
{
    [BurstCompile]
    public partial struct BodyRemovingJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Buffer;
        
        [BurstCompile]
        public void Execute([EntityIndexInQuery] int entityIndex, in Entity entity, in Mass mass)
        {
            if (mass.Value <= 0)
            {
                Buffer.DestroyEntity(entityIndex, entity);
            }
        }
    }
}