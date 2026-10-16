using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Server0162, Direction.ServerToClient)]
public sealed partial class Server0162
{
    [UInt(3)] public byte Unknown0 { get; set; }
}
