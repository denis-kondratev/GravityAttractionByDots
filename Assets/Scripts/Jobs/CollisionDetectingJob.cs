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
        [ReadOnly] public NativeArray<LocalTransform> Transforms;
        public NativeList<int2>.ParallelWriter Collisions;
        [ReadOnly] public float CollisionDistanceFactor;

        [BurstCompile]
        public void Execute(int index)
        {
            for (var i = index + 1; i < EntityCount; i++)
            {
                var sizeQ = Transforms[i].Scale * Transforms[i].Scale
                            + Transforms[index].Scale * Transforms[index].Scale;
                var distanceQ = math.distancesq(Transforms[i].Position, Transforms[index].Position);

                if (distanceQ < sizeQ * CollisionDistanceFactor)
                {
                    Collisions.AddNoResize(new int2(index, i));
                    break;
                }
            }
        }
    }
}