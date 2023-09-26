using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    [UpdateAfter(typeof(ProjectileMoveSystem))]
    public partial struct ProjectileHitSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //Get query for InitilPerProjectileJob to iterate over it
            var projectileQuery = SystemAPI.QueryBuilder().WithAll<ProjectileProperties, LocalToWorld>().Build();
            var projectileCount = projectileQuery.CalculateEntityCount();

            if (projectileCount == 0) { return; }

            var world = state.WorldUnmanaged;

            //Get query for AgentHitCheckJob to iterate over it
            var agentsQuery = SystemAPI.QueryBuilder().WithAspect<AgentLivesAspect>().Build();
            var projectileEntities = projectileQuery.ToEntityArray(world.UpdateAllocator.ToAllocator);

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();



            //Create projectile positions native array so it can be passed to distance checking jobs.
            var projectilePositions = CollectionHelper.CreateNativeArray<float3, RewindableAllocator>(projectileCount, ref world.UpdateAllocator);

            //Gets indexes of first entities in all of the chunks projectile archetype populates
            var projectileChunkBaseEntityIndexArray = projectileQuery.CalculateBaseEntityIndexArrayAsync(
                                                        world.UpdateAllocator.ToAllocator, state.Dependency,
                                                        out var projectileChunkBaseIndexJobHandle);

            //Fill up projectile positions array concurently. It won't crash via race conditions, thanks to indexes calculated above, so every job knows exactly were to put its data.
            var initialPerProjectileJob = new InitialPerProjectileJob
            {
                ChunkBaseEntityIndices = projectileChunkBaseEntityIndexArray,
                ProjectilePositions = projectilePositions,
            };
            //Schedule job with projectileChunkBaseIndexJobHandle passed as dependency, so it wont run before all of the chunkBaseIndexes have been calculated.
            var initialPerProjectileJobHandle = initialPerProjectileJob.ScheduleParallel(projectileQuery, projectileChunkBaseIndexJobHandle);

            var agentHitCheckJob = new AgentHitCheckJob
            {
                projectilesPositions = projectilePositions,
                projectilesEntities = projectileEntities,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            };

            //Thanks to passing initialPerProjectileJobHandle as a dependency to this handle, it won't run before all the Projectile Positions are copied.
            var agentHitCheckJobHandle = agentHitCheckJob.ScheduleParallel(agentsQuery, initialPerProjectileJobHandle);

            var initalPerProjectileAndAgentHitCheckJobHandle = JobHandle.CombineDependencies(initialPerProjectileJobHandle, agentHitCheckJobHandle);
            state.Dependency = initalPerProjectileAndAgentHitCheckJobHandle;
            agentsQuery.AddDependency(state.Dependency);
        }

        /// <summary>
        /// This basically fills out the native array of Projectile Positions multithreadedly
        /// </summary>
        [BurstCompile]
        partial struct InitialPerProjectileJob : IJobEntity
        {
            [ReadOnly] public NativeArray<int> ChunkBaseEntityIndices;
            [NativeDisableParallelForRestriction] public NativeArray<float3> ProjectilePositions;
            void Execute([ChunkIndexInQuery] int chunkIndexInQuery, [EntityIndexInChunk] int entityIndexInChunk, in LocalToWorld localToWorld)
            {
                int entityIndexInQuery = ChunkBaseEntityIndices[chunkIndexInQuery] + entityIndexInChunk;
                ProjectilePositions[entityIndexInQuery] = localToWorld.Position;
            }
        }

        [BurstCompile]
        partial struct AgentHitCheckJob : IJobEntity
        {
            [NativeDisableParallelForRestriction][ReadOnly] public NativeArray<float3> projectilesPositions;
            [NativeDisableParallelForRestriction][ReadOnly] public NativeArray<Entity> projectilesEntities;
            internal EntityCommandBuffer.ParallelWriter ECB;

            public void Execute([ChunkIndexInQuery] int chunkIndexInQuery, AgentLivesAspect agentLives)
            {
                for (int i = 0; i < projectilesPositions.Length; i++)
                {
                    var distanceSQ = math.distancesq(agentLives.Position, projectilesPositions[i]);
                    if (distanceSQ < 0.25f)
                    {
                        ECB.DestroyEntity(chunkIndexInQuery, projectilesEntities[i]);

                        agentLives.Lives--;

                        if (agentLives.Lives <= 0)
                        {
                            ECB.DestroyEntity(chunkIndexInQuery, agentLives.Entity);
                        }
                        else
                        {
                            agentLives.UniformScale = 0;
                            ECB.RemoveComponent<AgentAliveTag>(chunkIndexInQuery, agentLives.Entity);
                            ECB.AddComponent<AgentDeadTag>(chunkIndexInQuery, agentLives.Entity);
                            ECB.SetComponent(chunkIndexInQuery, agentLives.Entity, new AgentRespawnTimer { Value = 0 });
                            break;
                        }

                    }
                }
            }
        }
    }
}
