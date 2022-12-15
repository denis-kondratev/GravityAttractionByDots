using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace GravityAttraction
{
    public class BodyAuthoring : MonoBehaviour
    {
        public class Baker : Baker<BodyAuthoring>
        {
            public override void Bake(BodyAuthoring authoring)
            {
                AddComponent<Mass>();
                AddComponent<PhysicsVelocity>();
                AddComponent<Collision>();
            }
        }
    }
}