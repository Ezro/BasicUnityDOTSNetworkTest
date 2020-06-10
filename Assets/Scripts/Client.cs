using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Assets.Scripts
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class GoInGameClientSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.WithNone<NetworkStreamInGame>().ForEach((Entity ent, ref NetworkIdComponent id) =>
            {
                PostUpdateCommands.AddComponent<NetworkStreamInGame>(ent);
                var req = PostUpdateCommands.CreateEntity();
                PostUpdateCommands.AddComponent(req, new GoInGameRequest(Random.Range(1, 100)));
                PostUpdateCommands.AddComponent(req, new SendRpcCommandRequestComponent());
            });
        }
    }
}
