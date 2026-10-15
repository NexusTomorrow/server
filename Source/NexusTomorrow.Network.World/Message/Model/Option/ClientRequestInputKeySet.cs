using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[Message(GameMessageOpcode.ClientRequestInputKeySet)]
[WireMessage(GameMessageOpcode.ClientRequestInputKeySet, Direction.ClientToServer)]
public sealed partial class ClientRequestInputKeySet : IReadable
{
    [UInt(64)] public ulong CharacterId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        CharacterId = reader.ReadULong();
    }
}
