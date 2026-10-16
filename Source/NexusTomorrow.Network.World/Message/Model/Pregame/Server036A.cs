using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Server036A, Direction.ServerToClient)]
public sealed partial class Server036A
{
    [UInt(5)] public byte Unknown0 { get; set; }
}
