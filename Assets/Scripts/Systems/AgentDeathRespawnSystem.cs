using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    [UpdateAfter(typeof(ProjectileHitSystem))]
    public partial struct AgentDeathRespawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new RespawnTimerJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct RespawnTimerJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;

        [BurstCompile]
        private void Execute(Entity entity, 
            ref LocalTransform localTransform, 
            ref AgentRespawnTimer agentRespawnTimer, 
            in AgentRespawnProperties agentRespawnProperties,
            in AgentDeadTag _,
            [ChunkIndexInQuery] int sortKey)
        {
            agentRespawnTimer.Value += DeltaTime;

            if(agentRespawnTimer.Value > agentRespawnProperties.RespawnCooldown)
            {
                localTransform.Scale = 1;
                ECB.RemoveComponent<AgentDeadTag>(sortKey, entity);
                ECB.AddComponent<NewAgentTag>(sortKey, entity);
            }

        }
    }
}

