using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace GravityAttraction
{
    [BurstCompile]
    public partial struct CollisionDetectingJob : IJobParallelFor
    {
        [ReadOnly] public int EntityCount;
        [ReadOnly] public NativeArray<LocalToWorldTransform> Transforms;
        public NativeList<int2>.ParallelWriter Collisions;
        [ReadOnly] public float CollisionDistanceFactor;

        [BurstCompile]
        public void Execute(int index)
        {
            for (var i = index + 1; i < EntityCount; i++)
            {
                var sizeQ = Transforms[i].Value.Scale * Transforms[i].Value.Scale
                            + Transforms[index].Value.Scale * Transforms[index].Value.Scale;
                var distanceQ = math.distancesq(Transforms[i].Value.Position, Transforms[index].Value.Position);

                if (distanceQ < sizeQ * CollisionDistanceFactor)
                {
                    Collisions.AddNoResize(new int2(index, i));
                    break;
                }
            }
        }
    }
}