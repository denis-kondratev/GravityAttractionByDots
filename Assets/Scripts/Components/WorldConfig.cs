using System;
using Unity.Entities;
using Unity.Mathematics;

namespace GravityAttraction
{
    [Serializable]
    public struct WorldConfig : IComponentData
    {
        public int BodyCount;
        public float WorldRadius;
        public float2 StartSpeedRange;
        public float2 StartMassRange;
        public float GravitationalConstant;
        public float MinDistanceToAttract;
        public float MassToScaleRatio;
        public float CollisionFactor;
    }
}