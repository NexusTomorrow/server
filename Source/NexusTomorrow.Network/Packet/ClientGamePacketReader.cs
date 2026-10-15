using NexusTomorrow.Cryptography;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Packet
{
    public sealed class ClientGamePacketReader : IDisposable
    {
        private MemoryStream? stream;
        private GamePacketReader? reader;

        private GamePacketReader Reader => reader ?? throw new InvalidOperationException($"{nameof(reader)} is not set; call {nameof(Initialise)} first.");

        public void Dispose()
        {
            stream?.Dispose();
            reader?.Dispose();
        }

        public void Initialise(ClientGamePacket packet, PacketCrypt encryption)
        {
            if (stream != null || reader != null)
                throw new InvalidOperationException();

            stream = new MemoryStream(packet.IsEncrypted ? encryption.Decrypt(packet.Data, packet.Data.Length) : packet.Data);
            reader = new GamePacketReader(stream);
        }

        public GameMessageOpcode ReadHeader()
        {
            return Reader.ReadEnum<GameMessageOpcode>(16u);
        }

        public uint ReadBody(IReadable message)
        {
            message.Read(Reader);
            return Reader.BytesRemaining;
        }
    }
}
