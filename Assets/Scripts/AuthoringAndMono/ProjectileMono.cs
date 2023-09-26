using Unity.Entities;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public class ProjectileMono : MonoBehaviour
    {
        public float Speed = 5f;
    }

    public class ProjectileBaker : Baker<ProjectileMono>
    {
        public override void Bake(ProjectileMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<ProjectileProperties>(entity, new ProjectileProperties
            {
                Speed = authoring.Speed
            });
        }

    }
}
