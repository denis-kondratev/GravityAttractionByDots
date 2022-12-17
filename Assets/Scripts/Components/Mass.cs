using Unity.Entities;

namespace GravityAttraction
{
    public struct Mass : IComponentData, IEnableableComponent
    {
        public float Value;
    }
}