using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[WireMessage(GameMessageOpcode.Server013E, Direction.ServerToClient)]
public sealed partial class Server013E
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
