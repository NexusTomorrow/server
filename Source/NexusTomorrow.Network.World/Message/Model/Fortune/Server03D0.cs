using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[WireMessage(GameMessageOpcode.Server03D0, Direction.ServerToClient)]
public sealed partial class Server03D0
{
    [UInt(3)] public uint Unknown0 { get; set; }
}
