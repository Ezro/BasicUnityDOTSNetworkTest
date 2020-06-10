using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Assets.Scripts
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class SampleCubeInput : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<NetworkIdComponent>();
        }

        protected override void OnUpdate()
        {
            var localInput = GetSingleton<CommandTargetComponent>().targetEntity;
            if (localInput == Entity.Null)
            {
                var localPlayerId = GetSingleton<NetworkIdComponent>().Value;
                Entities.WithNone<CubeInput>().ForEach((Entity ent, ref MovableCubeComponent cube) =>
                {
                    if (cube.PlayerId == localPlayerId)
                    {
                        PostUpdateCommands.AddBuffer<CubeInput>(ent);
                        PostUpdateCommands.SetComponent(GetSingletonEntity<CommandTargetComponent>(), new CommandTargetComponent { targetEntity = ent });
                    }
                });
                return;
            }
            var input = default(CubeInput);
            input.tick = World.GetExistingSystem<ClientSimulationSystemGroup>().ServerTick;
            if (Input.GetKey("a"))
                input.Horizontal -= 1;
            if (Input.GetKey("d"))
                input.Horizontal += 1;
            if (Input.GetKey("s"))
                input.Vertical -= 1;
            if (Input.GetKey("w"))
                input.Vertical += 1;
            var inputBuffer = EntityManager.GetBuffer<CubeInput>(localInput);
            inputBuffer.AddCommandData(input);
        }
    }
}
