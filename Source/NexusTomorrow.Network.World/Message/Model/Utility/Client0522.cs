using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Client0522, Direction.ClientToServer)]
public sealed partial class Client0522
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
