using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

[Message(GameMessageOpcode.ClientEncrypted)]
[WireMessage(GameMessageOpcode.ClientEncrypted, Direction.ClientToServer)]
public sealed partial class ClientEncrypted : IReadable
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Data { get; private set; } = [];

    public void Read(GamePacketReader reader)
    {
        uint length = reader.ReadUInt();
        Data = reader.ReadBytes(length - 4);
    }
}
