using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ProjectContent.Scripts.Components
{
    public readonly partial struct MovementAspect : IAspect
    {
        private readonly Entity entity; // can only have one, auto sets the related entity as this value

        private readonly TransformAspect transformAspect; // our Aspect can only be a partial struct, and can only have readonly fields
        private readonly RefRO<SpeedComp> speedComp;
        private readonly RefRW<TargetComp> targetComp;

        public void Move(float deltaTime)
        {
            // ValueRW and ValueRO makes sure read right permission is defined on the value
            float3 direction = math.normalize(targetComp.ValueRW.value - transformAspect.LocalPosition);
            // cannot use SystemAPI inside IAspect, so deltaTime is a param
            transformAspect.LocalPosition += direction * deltaTime * speedComp.ValueRO.value;
        }
    }
}