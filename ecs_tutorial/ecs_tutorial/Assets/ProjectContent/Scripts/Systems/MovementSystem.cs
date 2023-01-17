using ProjectContent.Scripts.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace ProjectContent.Scripts.Systems
{
    [BurstCompile]
    public partial struct MovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            JobHandle jobHandle = new MoveJob
            {
                deltaTime = deltaTime
            }.ScheduleParallel(state.Dependency);
            jobHandle.Complete(); // if we want jobs afterwards run after this line rather than being parallel, then add this line
        }

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }


    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float deltaTime;

        [BurstCompile]
        public void Execute(MovementAspect movementAspect)
        {
            movementAspect.Move(deltaTime);
        }
    }
}