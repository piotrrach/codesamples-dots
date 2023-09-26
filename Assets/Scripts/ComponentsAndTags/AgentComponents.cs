using Unity.Entities;

namespace CodeSamplesDOTS
{
    public struct AgentRotateProperties : IComponentData
    {
        public float RotationSpeed;
    }

    public struct AgentRotatePropertiesDynamic : IComponentData
    {
        public float AngleTarget;
        public float StayTime;
        public float StayTimer;
        public float RotateTimer;
    }

    public struct AgentLives : IComponentData
    {
        public int Value;
    }

    public struct AgentShootProperties : IComponentData
    {
        public float ShootCooldown;
        public Entity ProjectileEntity;
        public float ProjectileSpawnOffset;
    }

    public struct AgentShootCooldown : IComponentData
    {
        public float Value;
    }

    public struct AgentRespawnProperties : IComponentData
    {
        public float RespawnCooldown;
    }

    public struct AgentRespawnTimer : IComponentData
    {
        public float Value;
    }

    public struct NewAgentTag : IComponentData { }
    public struct AgentAliveTag : IComponentData { }
    public struct AgentDeadTag : IComponentData { }
}
