using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace GravityAttraction
{
    [BurstCompile]
    public partial struct InitializingJob : IJobEntity
    {
        public WorldConfig WorldConfig;
        public float SqrtWorldRadius;
        public float MassToScaleRatio;

        [BurstCompile]
        private void Execute(
            [EntityIndexInQuery] int index,
            ref LocalTransform transform,
            ref Mass mass,
            ref PhysicsVelocity velocity)
        {
            var random = new Random();
            random.InitState((uint)index + 1u);
            
            // Setting random mass and scale (size).
            mass.Value = random.NextFloat(WorldConfig.StartMassRange.x, WorldConfig.StartMassRange.y);
            transform.Scale = Scaling.MassToScale(mass.Value, MassToScaleRatio);

            // Setting random position.
            var radius = WorldConfig.WorldRadius - math.pow(random.NextFloat(SqrtWorldRadius), 3);
            var position = random.NextFloat3Direction() * radius;
            transform.Position = position;

            // Setting random velocity.
            var speed = random.NextFloat(WorldConfig.StartSpeedRange.x, WorldConfig.StartSpeedRange.y);
            var direction = math.normalize(new float3(-position.z, position.y / 2, position.x));
            velocity.Linear = speed * direction;
        }
    }
}