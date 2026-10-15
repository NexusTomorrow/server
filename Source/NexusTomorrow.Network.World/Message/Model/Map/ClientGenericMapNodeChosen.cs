using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.ClientGenericMapNodeChosen, Direction.ClientToServer)]
public sealed partial class ClientGenericMapNodeChosen
{
    [UInt(14)] public ushort GenericMapNodeId { get; private set; }
}
