using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Server0765, Direction.ServerToClient)]
public sealed partial class Server0765
{
    [UInt(6)] public byte Unknown0 { get; set; }
}
