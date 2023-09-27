using Unity.Burst;
using Unity.Entities;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
    public partial class GameManageSystem : SystemBase
    {
        private bool _isInitialized = false;
        private bool _isGameStarted;
        private int _updatesAfterGameStarted;

        protected override void OnCreate()
        {
            RequireForUpdate<GameStateTag>();

            _isInitialized = false;
            _isGameStarted = false;
        }

        protected override void OnDestroy()
        {
            //Avoidance of errors upon ending playmode in editor
            if (UiFacadeSingleton.Instance == null)
            {
                return;
            }

            UiFacadeSingleton.Instance.OnStartPressed -= SetGameStarted;
            UiFacadeSingleton.Instance.OnAgentQuantityChoosen -= OnInitAgentQuantityChange;

        }

        protected override void OnUpdate()
        {
            var gameStateEntity = SystemAPI.GetSingletonEntity<GameStateTag>();

            if (!_isInitialized)
            {
                UiFacadeSingleton.Instance.OnStartPressed += SetGameStarted;
                UiFacadeSingleton.Instance.OnAgentQuantityChoosen += OnInitAgentQuantityChange;
                _isInitialized = true;
                return;
            }

            if (!_isGameStarted)
            {
                return;
            }

            if (!SystemAPI.HasSingleton<GameStartedTag>())
            {
                World.EntityManager.RemoveComponent<GameEndedTag>(gameStateEntity);
                World.EntityManager.AddComponent<GameStartedTag>(gameStateEntity);
            };

            _updatesAfterGameStarted++;

            if (_updatesAfterGameStarted <= 2)
            {
                return;
            }

            var agentsCount = GetEntityQuery(typeof(AgentLives)).CalculateEntityCount();
            if (agentsCount <= 1 && _isGameStarted && _updatesAfterGameStarted > 1)
            {
                UiFacadeSingleton.Instance.ShowGameOverWindow();
                _isGameStarted = false;

                World.EntityManager.RemoveComponent<GameStartedTag>(gameStateEntity);
                World.EntityManager.AddComponent<GameEndedTag>(gameStateEntity);
            }
        }

        private void SetGameStarted()
        {
            _isGameStarted = true;
            _updatesAfterGameStarted = 0;
        }

        private void OnInitAgentQuantityChange(int newAgentQuantity)
        {
            var fieldEntity = SystemAPI.GetSingletonEntity<FieldProperties>();
            var fieldProperties = SystemAPI.GetComponentRW<FieldProperties>(fieldEntity);

            fieldProperties.ValueRW.NumberAgentsToSpawn = newAgentQuantity;
        }
    }
}


