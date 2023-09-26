using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CodeSamplesDOTS
{
    public class FieldMono : MonoBehaviour
    {
        public GameObject AgentPrefab;

        public uint RandomSeed;
    }

    public class FieldBaker : Baker<FieldMono>
    {
        public override void Bake(FieldMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FieldProperties()
            {
                AgentPrefab = GetEntity(authoring.AgentPrefab, TransformUsageFlags.Dynamic),
            });
            AddComponent(entity, new FieldRandom
            {
                Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
            });
            AddComponent<SpawnPoints>(entity);
        }

    }
}
