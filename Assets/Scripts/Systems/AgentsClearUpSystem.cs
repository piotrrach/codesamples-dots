using Unity.Burst;
using Unity.Entities;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    public partial struct AgentsClearUpSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<GameEndedTag>())
            {
                return;
            }

            var agentsLeft = SystemAPI.QueryBuilder().WithAll<AgentLives>().Build();

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            ecb.DestroyEntity(agentsLeft, EntityQueryCaptureMode.AtRecord);
        }
    }

}

