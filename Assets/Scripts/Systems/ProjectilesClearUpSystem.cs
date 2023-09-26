using Unity.Burst;
using Unity.Entities;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    public partial struct ProjectilesClearUpSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<GameEndedTag>())
            {
                return;
            }

            var projectilesLeft = SystemAPI.QueryBuilder().WithAll<ProjectileProperties>().Build();

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            ecb.DestroyEntity(projectilesLeft, EntityQueryCaptureMode.AtRecord);
        }
    }

}

