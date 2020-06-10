using Unity.NetCode;
using Unity.Networking.Transport;

namespace Assets.Scripts
{
    public struct CubeInput : ICommandData<CubeInput>
    {
        public uint Tick => tick;
        public uint tick;
        public int Horizontal;
        public int Vertical;

        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteInt(Horizontal);
            writer.WriteInt(Vertical);
        }

        public void Deserialize(uint tick, ref DataStreamReader reader)
        {
            this.tick = tick;
            Horizontal = reader.ReadInt();
            Vertical = reader.ReadInt();
        }

        public void Serialize(ref DataStreamWriter writer, CubeInput baseline, NetworkCompressionModel compressionModel)
        {
            writer.WriteInt(Horizontal);
            writer.WriteInt(Vertical);
        }

        public void Deserialize(uint tick, ref DataStreamReader reader, CubeInput baseline, NetworkCompressionModel compressionModel)
        {
            Deserialize(tick, ref reader);
        }
    }

    public class CubeSendCommandSystem : CommandSendSystem<CubeInput> { }
    public class CubeReceiveCommandSystem : CommandReceiveSystem<CubeInput> { }
}
