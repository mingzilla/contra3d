using ProjectContent.Scripts.Components;
using Unity.Entities;

namespace ProjectContent.Scripts.Systems
{
    public partial class MovementSystemBase : SystemBase
    {
        protected override void OnUpdate()
        {
            return; // to disable this SystemBase

            foreach (MovementAspect movementAspect in SystemAPI.Query<MovementAspect>())
            {
                movementAspect.Move(SystemAPI.Time.DeltaTime);
            }
        }
    }
}