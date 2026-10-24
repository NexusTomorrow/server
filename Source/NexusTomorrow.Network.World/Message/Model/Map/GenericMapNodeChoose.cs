using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Client0754, Direction.ClientToServer)]
public sealed partial class GenericMapNodeChoose
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
}
