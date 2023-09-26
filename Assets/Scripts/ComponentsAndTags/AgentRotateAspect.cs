using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeSamplesDOTS
{
    public readonly partial struct AgentRotateAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<AgentRotateProperties> _rotateProperties;
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<AgentRotatePropertiesDynamic> _rotatePropertiesDynamic;

        public float StayTime
        {
            get => _rotatePropertiesDynamic.ValueRO.StayTime;
            set => _rotatePropertiesDynamic.ValueRW.StayTime = value;
        }

        public float StayTimer
        {
            get => _rotatePropertiesDynamic.ValueRO.StayTimer;
            set => _rotatePropertiesDynamic.ValueRW.StayTimer = value;
        }

        public float RotateTimer
        {
            get => _rotatePropertiesDynamic.ValueRO.RotateTimer;
            set => _rotatePropertiesDynamic.ValueRW.RotateTimer = value;
        }

        public float AngleTarget
        {
            get => _rotatePropertiesDynamic.ValueRO.AngleTarget;
            set => _rotatePropertiesDynamic.ValueRW.AngleTarget = value;
        }

        public float AngleRotated => RotateTimer * _rotateProperties.ValueRO.RotationSpeed;
        public bool IsTargetAngleReached => AngleRotated >= AngleTarget;
        public bool IsStayTimeOver => StayTimer >= StayTime;

        public void RotateRight(float deltaTime)
        {
            RotateTimer += deltaTime;
            _transform.ValueRW.Rotation = quaternion.Euler(new float3(0, 0, AngleRotated));
        }

        public void StayInPlace(float deltaTime)
        {
            StayTimer += deltaTime;
        }

        public void ResetStayTimer()
        {
            StayTimer = 0;
        }


    }
}
