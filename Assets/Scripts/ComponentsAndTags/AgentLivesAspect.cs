using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeSamplesDOTS
{
    public readonly partial struct AgentLivesAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _localTransform;
        private readonly RefRW<AgentLives> _lives;

        public float UniformScale
        {
            get => _localTransform.ValueRO.Scale;
            set => _localTransform.ValueRW.Scale = value;
        }

        public int Lives
        {
            get => _lives.ValueRO.Value;
            set => _lives.ValueRW.Value = value;
        }
        public float3 Position
        {
            get => _localTransform.ValueRO.Position;
            set => _localTransform.ValueRW.Position = value;
        }


    }
}
