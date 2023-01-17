using Unity.Entities;
using Unity.Mathematics;

namespace ProjectContent.Scripts.Components
{
    public struct TargetComp : IComponentData
    {
        public float3 value;
    }
}