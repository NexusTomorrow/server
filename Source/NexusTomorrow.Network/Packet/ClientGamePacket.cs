namespace NexusTomorrow.Network.Packet;

public class ClientGamePacket
{
    public bool IsEncrypted { get; init; }
    public required byte[] Data { get; init; }
}
