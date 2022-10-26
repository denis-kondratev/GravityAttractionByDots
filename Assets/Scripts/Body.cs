using Unity.Entities;
using UnityEngine;

namespace GravityAttraction
{
    public class Body : MonoBehaviour
    {
        public class BodyBaker : Baker<Body>
        {
            public override void Bake(Body authoring)
            {
                AddComponent<Velocity>();
                AddComponent<Mass>();
            }
        }
    }
}