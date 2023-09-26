using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CodeSamplesDOTS
{
    [BurstCompile]
    public partial class CameraControllerSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<FieldProperties>();
        }

        protected override void OnUpdate()
        {
            var fieldEntity = SystemAPI.GetSingletonEntity<FieldProperties>();
            var fieldAspect = SystemAPI.GetAspect<FieldAspect>(fieldEntity);

            var farthestAgentPosition = fieldAspect.GetFarthestAgentPosition();
            var distanceToFarthest = math.distance(float3.zero, farthestAgentPosition) + 1f;
            Camera.main.orthographicSize = distanceToFarthest;
        }
    }
}