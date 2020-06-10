using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Assets.Scripts
{
    [UpdateInGroup(typeof(GhostPredictionSystemGroup))]
    public class MovableCubeSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            var group = World.GetExistingSystem<GhostPredictionSystemGroup>();
            var tick = group.PredictingTick;
            var deltaTime = group.Time.DeltaTime;
            Entities.ForEach((DynamicBuffer<CubeInput> inputBuffer, ref Translation trans, ref PredictedGhostComponent prediction) =>
            {
                if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
                    return;
                CubeInput input;
                inputBuffer.GetDataAtTick(tick, out input);
                if (input.Horizontal > 0)
                    trans.Value.x += deltaTime;
                if (input.Horizontal < 0)
                    trans.Value.x -= deltaTime;
                if (input.Vertical > 0)
                    trans.Value.y += deltaTime;
                if (input.Vertical < 0)
                    trans.Value.y -= deltaTime;
            });
        }
    }
}
