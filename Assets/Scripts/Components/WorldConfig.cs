using System;
using Unity.Entities;
using Unity.Mathematics;

namespace GravityAttraction
{
    [Serializable]
    public struct WorldConfig : IComponentData
    {
        public Entity BodyPrefab;
        public int BodyCount;
        public float WorldRadius;
        public float StartMaxAngularSpeed;
        public float2 StartSpeedRange;
        public float2 StartMassRange;
        public float GravitationalConstant;
        public float MinDistanceToAttract;
        public float MassToSizeFactor;
        public float CollisionDistanceFactor;
    }
}