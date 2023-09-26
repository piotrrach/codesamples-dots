using Unity.Burst;
using Unity.Entities;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    [UpdateAfter(typeof(AgentShootSystem))]
    public partial struct ProjectileMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var agentsAlive = SystemAPI.QueryBuilder().WithAll<AgentRotateProperties>().Build().CalculateEntityCount();

            if (agentsAlive <= 1)
            {
                return;
            }

            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            new ProjectileMoveJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }


    [BurstCompile]
    public partial struct ProjectileMoveJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;

        [BurstCompile]
        private void Execute(ProjectileAspect aspect, [ChunkIndexInQuery] int sortKey)
        {
            aspect.Move(DeltaTime);
            if (aspect.IsOutsideOfBounds)
            {
                ECB.DestroyEntity(sortKey, aspect.Entity);
            }
        }
    }
}

