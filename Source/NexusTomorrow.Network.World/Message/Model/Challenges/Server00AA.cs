using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[WireMessage(GameMessageOpcode.Server00AA, Direction.ServerToClient)]
public sealed partial class Server00AA
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
