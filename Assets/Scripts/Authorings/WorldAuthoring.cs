using Unity.Entities;
using UnityEngine;

namespace GravityAttraction
{
    public class WorldAuthoring : MonoBehaviour
    {
        public GameObject BodyPrefab;
        public WorldConfig WorldConfig;

        public class WorldConfigBaker : Baker<WorldAuthoring>
        {
            public override void Bake(WorldAuthoring authoring)
            {
                AddComponent(new BodyPrefab
                {
                    Value = GetEntity(authoring.BodyPrefab)
                });
                
                AddComponent(authoring.WorldConfig);
            }
        }
    }
}