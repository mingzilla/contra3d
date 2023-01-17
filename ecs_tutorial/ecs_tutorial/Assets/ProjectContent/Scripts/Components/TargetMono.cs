using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectContent.Scripts.Components
{
    public class TargetMono : MonoBehaviour
    {
        public float3 value;
    }

    public class TargetBaker : Baker<TargetMono>
    {
        public override void Bake(TargetMono mono)
        {
            AddComponent(new TargetComp
            {
                value = mono.value
            });
        }
    }
}