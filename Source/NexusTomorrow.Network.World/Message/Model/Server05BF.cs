using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server05BF, Direction.ServerToClient)]
public sealed partial class Server05BF
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
