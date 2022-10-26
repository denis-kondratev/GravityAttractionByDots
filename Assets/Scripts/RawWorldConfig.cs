using Unity.Entities;
using UnityEngine;

namespace GravityAttraction
{
    public class RawWorldConfig : MonoBehaviour
    {
        public GameObject BodyPrefab;
        public WorldConfig WorldConfig;

        public class WorldConfigBaker : Baker<RawWorldConfig>
        {
            public override void Bake(RawWorldConfig authoring)
            {
                authoring.WorldConfig.BodyPrefab = GetEntity(authoring.BodyPrefab);
                AddComponent(authoring.WorldConfig);
            }
        }
    }
}