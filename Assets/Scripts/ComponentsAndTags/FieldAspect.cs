using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeSamplesDOTS
{
    public readonly partial struct FieldAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<FieldProperties> _properties;
        private readonly RefRW<FieldRandom> _random;
        private readonly RefRO<SpawnPoints> _spawnPoints;

        public Entity AgentPrefab => _properties.ValueRO.AgentPrefab;

        public int SpawnPointsCount => _spawnPoints.ValueRO.Value.Value.Value.Length;

        public int AgentsToSpawn => _properties.ValueRO.NumberAgentsToSpawn;

        public bool IsTimeToSpawn => true;

        public float3 GetSpawnPoint(int index)
        {
            return _spawnPoints.ValueRO.Value.Value.Value[index];
        }

        public float3 GetFarthestAgentPosition()
        {
            if(AgentsToSpawn <= 0)
            {
                return float3.zero;
            }
            return GetSpawnPoint(AgentsToSpawn - 1);
        }
    }
}
