using Unity.Entities;

namespace Assets.Scripts
{
    [GenerateAuthoringComponent]
    public struct MovableCubeComponent : IComponentData
    {
        public int PlayerId;
    }
}
