using UnityEngine;
using Unity.Entities;

namespace ProjectContent.Scripts.Components
{
    public class SpeedMono : MonoBehaviour
    {
        public float value;
    }

    public class SpeedBaker : Baker<SpeedMono>
    {
        public override void Bake(SpeedMono mono)
        {
            AddComponent(new SpeedComp
            {
                value = mono.value
            });
        }
    }
}