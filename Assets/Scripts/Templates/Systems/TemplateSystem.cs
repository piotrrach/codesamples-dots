using Unity.Burst;
using Unity.Entities;

namespace CodeSamplesDOTS
{
    [BurstCompile]

    public partial struct TemplateSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            new TemplateJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Run();
        }
    }

    [BurstCompile]
    public partial struct TemplateJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;

        [BurstCompile]
        private void Execute(TemplateAspect aspect)
        {

        }
    }
}
