using Unity.Burst;
using Unity.Entities;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitializeAgentSpawnSystem : ISystem
    {
        private EndInitializationEntityCommandBufferSystem.Singleton _ecbSingleton;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _ecbSingleton = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = _ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var agent in SystemAPI.Query<AgentLivesAspect>().WithAll<NewAgentTag>())
            {
                ecb.RemoveComponent<NewAgentTag>(agent.Entity);
                ecb.AddComponent<AgentAliveTag>(agent.Entity);
            }
        }
    }
}