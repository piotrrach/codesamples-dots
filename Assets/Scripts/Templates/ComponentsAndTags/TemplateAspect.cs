using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public readonly partial struct TemplateAspect : IComponentData
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;

        public void Move(float3 position)
        {
            _transform.ValueRW.Position = position;
        }
    }
}
