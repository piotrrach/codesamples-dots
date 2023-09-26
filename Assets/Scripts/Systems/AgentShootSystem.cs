using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    public partial struct AgentShootSystem : ISystem
    {
        private float timestared;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            timestared = (float)SystemAPI.Time.ElapsedTime;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if(timestared > 5f)
            {
                return;
            }

            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            new ShootProjectileJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ShootProjectileJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;

        [BurstCompile]
        private void Execute(AgentShootAspect agentShootAspect, in AgentAliveTag _, [ChunkIndexInQuery] int sortKey)
        {
            if (agentShootAspect.IsProjectileReady)
            {
                agentShootAspect.ResetCooldown();
                var newProjectile = ECB.Instantiate(sortKey, agentShootAspect.ProjectilePrefab);

                var spawnPosition = agentShootAspect.ProjectileSpawnPosition;
                float heading = MathHelpers.GetHeading(agentShootAspect.Position, spawnPosition);

                LocalTransform newProjectileTransform = new LocalTransform
                {
                    Position = spawnPosition,
                    Rotation = quaternion.RotateZ(heading),
                    Scale = 1
                };

                ECB.SetComponent(sortKey, newProjectile, newProjectileTransform);
            }
            else
            {
                agentShootAspect.LowerCooldown(DeltaTime);
            }
        }
    }


}

