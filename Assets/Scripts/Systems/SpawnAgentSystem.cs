using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    public partial struct SpawnAgentSystem : ISystem
    {
        private bool _shouldSpawnAgents;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FieldProperties>();
            state.RequireForUpdate<GameStateTag>();
            _shouldSpawnAgents = true;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.HasSingleton<GameEndedTag>())
            {
                _shouldSpawnAgents = true;
            }

            if (!SystemAPI.HasSingleton<GameStartedTag>())
            {
                return;
            }

            if (!_shouldSpawnAgents)
            {
                return;
            }


            var fieldEntity = SystemAPI.GetSingletonEntity<FieldProperties>();
            var fieldAspect = SystemAPI.GetAspect<FieldAspect>(fieldEntity);


            SpawnAgents(ref fieldAspect, ref state);

            _shouldSpawnAgents = false;
        }


        [BurstCompile]
        private void SpawnAgents(ref FieldAspect fieldAspect, ref SystemState state)
        {
            if (fieldAspect.AgentsToSpawn <= 0 || fieldAspect.SpawnPointsCount == 0)
            {
                return;
            }

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();

            for (int i = 0; i < fieldAspect.AgentsToSpawn; i++)
            {
                new SpawnAgentJob
                {
                    ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                    Index = i
                }.ScheduleParallel();
            }
        }
    }

    [BurstCompile]
    public partial struct SpawnAgentJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public int Index;

        [BurstCompile]
        private void Execute(FieldAspect aspect, [ChunkIndexInQuery] int sortKey)
        {
            var newAgent = ECB.Instantiate(sortKey, aspect.AgentPrefab);

            ECB.SetComponent(sortKey, newAgent, new LocalTransform()
            {
                Position = aspect.GetSpawnPoint(Index),
                Rotation = Unity.Mathematics.quaternion.identity,
                Scale = 1f
            }); ;
        }
    }
}

