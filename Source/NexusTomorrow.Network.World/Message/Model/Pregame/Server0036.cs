using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Server0036, Direction.ServerToClient)]
public sealed partial class Server0036
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
