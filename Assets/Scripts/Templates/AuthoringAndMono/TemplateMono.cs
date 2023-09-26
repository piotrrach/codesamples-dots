using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public class TemplateMono : MonoBehaviour
    {

    }

    public class TemplateBaker : Baker<TemplateMono>
    {
        public override void Bake(TemplateMono authoring)
        {
            //var templateEntity = GetEntity(TransformUsageFlags.Dynamic);
            //AddComponent<TemplateProperties>(templateEntity);
        }
    }
}
