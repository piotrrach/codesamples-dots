using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeSamplesDOTS
{
    public readonly partial struct ProjectileAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<ProjectileProperties> _properties;

        public bool IsOutsideOfBounds => math.abs(_transform.ValueRO.Position.x) > 100 || math.abs(_transform.ValueRO.Position.y) > 100;  
        
        public float3 Position => _transform.ValueRO.Position;

        public void Move(float DeltaTime)
        {
            _transform.ValueRW.Position += _transform.ValueRO.Up() * _properties.ValueRO.Speed * DeltaTime;
        }

    }
}
