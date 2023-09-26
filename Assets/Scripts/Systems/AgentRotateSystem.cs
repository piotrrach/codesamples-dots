using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    public partial struct AgentRotateSystem : ISystem
    {
        private Random _random;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _random = Random.CreateFromIndex(1234);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            new RotateAgentJob
            {
                DeltaTime = deltaTime,
                Random = _random
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct RotateAgentJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        public Random Random;

        [BurstCompile]
        private void Execute(AgentRotateAspect agentRotateAspect, in AgentAliveTag _)
        {
            if (!agentRotateAspect.IsTargetAngleReached)
            {
                agentRotateAspect.RotateRight(DeltaTime);
            }
            else
            {
                if (agentRotateAspect.IsStayTimeOver)
                {
                    var angle = Random.NextFloat(0,MathHelpers.PI2);
                    agentRotateAspect.AngleTarget += angle;

                    agentRotateAspect.StayTime = Random.NextFloat(0, 1);
                    agentRotateAspect.ResetStayTimer();
                    return;
                }
                agentRotateAspect.StayInPlace(DeltaTime);
            }
        }
    }
}

