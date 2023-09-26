using Unity.Entities;
using Unity.Mathematics;

namespace CodeSamplesDOTS
{
    public struct FieldRandom : IComponentData
    {
        public Random Value;
    }
}
