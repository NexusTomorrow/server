using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server0186, Direction.ServerToClient)]
public sealed partial class Server0186
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
