namespace NexusTomorrow.Network.Packet
{
    public class ClientGamePacket
    {
        public bool IsEncrypted { get; init; }
        public byte[] Data { get; init; }
    }
}
