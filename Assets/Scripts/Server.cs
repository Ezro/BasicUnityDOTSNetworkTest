using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Assets.Scripts
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    public class GoInGameServerSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.WithNone<SendRpcCommandRequestComponent>().ForEach((Entity entity, ref GoInGameRequest cmd, ref ReceiveRpcCommandRequestComponent req) =>
            {
                PostUpdateCommands.AddComponent<NetworkStreamInGame>(req.SourceConnection);
                Debug.Log($"We received a command! {cmd.Value} {req.SourceConnection}");

                var ghostCollection = GetSingleton<GhostPrefabCollectionComponent>();
                var ghostId = BasicUnityDOTSNetworkTestGhostSerializerCollection.FindGhostType<CubeSnapshotData>();
                var prefab = EntityManager.GetBuffer<GhostPrefabBuffer>(ghostCollection.serverPrefabs)[ghostId].Value;
                var player = EntityManager.Instantiate(prefab);
                EntityManager.SetComponentData(player, new MovableCubeComponent { PlayerId = EntityManager.GetComponentData<NetworkIdComponent>(req.SourceConnection).Value });
                PostUpdateCommands.AddBuffer<CubeInput>(player);
                PostUpdateCommands.SetComponent(req.SourceConnection, new CommandTargetComponent { targetEntity = player });
                PostUpdateCommands.DestroyEntity(entity);
            });
        }
    }
}
