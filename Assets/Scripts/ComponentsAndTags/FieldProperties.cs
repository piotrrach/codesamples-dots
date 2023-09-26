using Unity.Entities;
using Unity.Mathematics;

namespace CodeSamplesDOTS
{
    public struct FieldProperties : IComponentData
    {
        public Entity AgentPrefab;
        public int NumberAgentsToSpawn;
    }
}
