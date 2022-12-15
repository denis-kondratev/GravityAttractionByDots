using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace GravityAttraction
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(AttractingSystem))]
    public partial struct CollisionHandlingSystem : ISystem
    {
        private ComponentLookup<Mass> _massLookup;
        private ComponentLookup<LocalTransform> _transformLookup;
        private ComponentLookup<PhysicsVelocity> _velocityLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _massLookup = SystemAPI.GetComponentLookup<Mass>();
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
            _velocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>();
        }

        public void OnDestroy(ref SystemState state) { }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _massLookup.Update(ref state);
            _transformLookup.Update(ref state);
            _velocityLookup.Update(ref state);
            var massToScaleRatio = SystemAPI.GetSingleton<WorldConfig>().MassToScaleRatio;
            
            foreach (var (velocityRef, massRef, transformRef, collisionsRef) 
                     in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<Mass>, RefRW<LocalTransform>, RefRO<Collision>>())
            {
                var anotherBodyEntity = collisionsRef.ValueRO.Entity;
                var m1 = massRef.ValueRO.Value;

                if (m1 <= 0 || anotherBodyEntity == Entity.Null)
                {
                    continue;
                }
                
                var x1 = transformRef.ValueRO.Position;
                var v1 = velocityRef.ValueRO.Linear;
                
                var anotherMassRef = _massLookup.GetRefRW(anotherBodyEntity, false);
                var m2 = anotherMassRef.ValueRO.Value;

                if (m2 <= 0)
                {
                    continue;
                }
            
                var x2 = _transformLookup.GetRefRO(anotherBodyEntity).ValueRO.Position;
                var v2 = _velocityLookup.GetRefRO(anotherBodyEntity).ValueRO.Linear;

                x1 = (x1 * m1 + x2 * m2) / (m1 + m2);
                v1 = (v1 * m1 + v2 * m2) / (m1 + m2);
                m1 += m2;
                anotherMassRef.ValueRW.Value = 0;
                
                velocityRef.ValueRW.Linear = v1;
                massRef.ValueRW.Value = m1;
                
                transformRef.ValueRW = new LocalTransform
                {
                    Position = x1,
                    Rotation = quaternion.identity,
                    Scale = Scaling.MassToScale(m1, massToScaleRatio)
                };
            }
        }
    }
}