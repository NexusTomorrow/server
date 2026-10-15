using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[Message(GameMessageOpcode.ClientGenericMapNodeChosen)]
[WireMessage(GameMessageOpcode.ClientGenericMapNodeChosen, Direction.ClientToServer)]
public sealed partial class ClientGenericMapNodeChosen : IReadable
{
    [UInt(14)] public ushort GenericMapNodeId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        GenericMapNodeId = reader.ReadUShort(14u);
    }
}
