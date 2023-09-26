using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public readonly partial struct AgentShootAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> _transform;
        private readonly RefRO<AgentShootProperties> _properties;
        private readonly RefRW<AgentShootCooldown> _shootCooldown;

        public bool IsProjectileReady => _shootCooldown.ValueRO.Value <= 0;
        public Entity ProjectilePrefab => _properties.ValueRO.ProjectileEntity;

        public float3 Right => _transform.ValueRO.Right();
        public float3 ProjectileSpawnPosition => _transform.ValueRO.Position + 
            _transform.ValueRO.Up() * _properties.ValueRO.ProjectileSpawnOffset;

        public float3 Position => _transform.ValueRO.Position;

        public void LowerCooldown(float deltaTime)
        {
            _shootCooldown.ValueRW.Value -= deltaTime;
        }

        public void ResetCooldown()
        {
            _shootCooldown.ValueRW.Value = _properties.ValueRO.ShootCooldown;
        }
    }
}
