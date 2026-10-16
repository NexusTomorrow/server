using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0129, Direction.ServerToClient)]
public sealed partial class Server0129
{
    [UInt(18)] public uint Unknown0 { get; set; }
}

