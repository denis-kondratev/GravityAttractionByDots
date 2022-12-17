using System;
using Unity.Entities;
using Unity.Mathematics;

namespace GravityAttraction
{
    [Serializable]
    public struct WorldConfig : IComponentData
    {
        /// <summary>
        /// Initial count of bodies.
        /// </summary>
        public int BodyCount;
        
        /// <summary>
        /// The radius of the sphere within which the bodies will be created during initialization.
        /// </summary>
        public float WorldRadius;
        
        /// <summary>
        /// The range of initial speed of bodies.
        /// </summary>
        public float2 StartSpeedRange;
        
        /// <summary>
        /// The range of initial mass of bodies.
        /// </summary>
        public float2 StartMassRange;
        
        /// <summary>
        /// The coefficient of increasing the gravitational power.
        /// </summary>
        public float GravitationalConstant;
        
        /// <summary>
        /// The zero distance between objects can lead to infinite force. To avoid the occurrence of extremely large forces
        /// during the calculation, add a restriction on the minimum distance.
        /// </summary>
        public float MinDistanceToAttract;
        
        /// <summary>
        /// The ratio of the body mass to its size. So objects with different masses have different sizes.
        /// </summary>
        public float MassToScaleRatio;
        
        /// <summary>
        /// The coefficient that will determine how close the objects should be to each other to make the collision.
        /// </summary>
        public float CollisionFactor;
    }
}