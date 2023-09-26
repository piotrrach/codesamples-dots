using Unity.Entities;
using Unity.Mathematics;

public struct SpawnPoints : IComponentData
{
    public BlobAssetReference<SpawnPointsBlob> Value;
}

public struct SpawnPointsBlob
{
    public BlobArray<float3> Value;
}