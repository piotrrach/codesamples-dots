using Unity.Entities;

namespace CodeSamplesDOTS
{
    public struct GameStateTag : IComponentData { }

    public struct GameStartedTag : IComponentData {}

    public struct GameEndedTag : IComponentData { }
}
