﻿using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace GravityAttraction
{
    [BurstCompile]
    public partial struct BodyInitializingJob : IJobEntity
    {
        public Random Random;
        public WorldConfig WorldConfig;
        public float SqrtWorldRadius;
        
        public void Execute(
            ref LocalToWorldTransform transform,
            ref Mass mass,
            ref Velocity velocity)
        {
            var position = Random.NextFloat3Direction() * (WorldConfig.WorldRadius - math.pow(Random.NextFloat(SqrtWorldRadius), 3));
            transform.Value.Position = position;
            transform.Value.Rotation = quaternion.Euler(-position.z, 0, position.x);
            velocity.Value = math.normalize(new float3(-position.z, 0, position.x)) 
                             * Random.NextFloat(WorldConfig.StartSpeedRange.x, WorldConfig.StartSpeedRange.y);
            mass.Value = Random.NextFloat(WorldConfig.StartMassRange.x, WorldConfig.StartMassRange.y);
        }
    }
}