using Unity.Entities;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public class AgentMono : MonoBehaviour
    {
        public float RotationSpeed;

        public int Lives;
        public float RespawnCooldown;

        public GameObject ProjectilePrefab;
        public float ProjectileSpawnOffset;
        public float ShootCooldown;
    }

    public class AgentBaker : Baker<AgentMono>
    {
        public override void Bake(AgentMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<NewAgentTag>(entity);
            //rotating
            AddComponent(entity, new AgentRotateProperties
            {
                RotationSpeed = authoring.RotationSpeed,
            });
            AddComponent<AgentRotatePropertiesDynamic>(entity);

            //lives
            AddComponent(entity, new AgentLives
            {
                Value = authoring.Lives,
            });
            AddComponent(entity, new AgentRespawnProperties
            {
                RespawnCooldown = authoring.RespawnCooldown,
            });
            AddComponent<AgentRespawnTimer>(entity);

            //shooting
            AddComponent(entity, new AgentShootProperties
            {
                ProjectileEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
                ProjectileSpawnOffset = authoring.ProjectileSpawnOffset,
                ShootCooldown = authoring.ShootCooldown
            });
            AddComponent(entity, new AgentShootCooldown
            {
                Value = authoring.ShootCooldown,
            });
        }
    }
}