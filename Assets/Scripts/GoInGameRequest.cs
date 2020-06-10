using Unity.Burst;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace Assets.Scripts
{
    [BurstCompile]
    public struct GoInGameRequest : IRpcCommand
    {
        public int Value;
        
        public GoInGameRequest(int value)
        {
            Value = value;
        }

        public PortableFunctionPointer<RpcExecutor.ExecuteDelegate> CompileExecute()
        {
            return InvokeExecuteFunctionPointer;
        }

        public void Deserialize(ref DataStreamReader reader)
        {
            Value = reader.ReadInt();
        }

        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteInt(Value);
        }

        [BurstCompile]
        private static void InvokeExecute(ref RpcExecutor.Parameters parameters)
        {
            RpcExecutor.ExecuteCreateRequestComponent<GoInGameRequest>(ref parameters);
        }

        static PortableFunctionPointer<RpcExecutor.ExecuteDelegate> InvokeExecuteFunctionPointer = new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(InvokeExecute);
    }

    public class GoInGameRequestSystem : RpcCommandRequestSystem<GoInGameRequest> { }
}
