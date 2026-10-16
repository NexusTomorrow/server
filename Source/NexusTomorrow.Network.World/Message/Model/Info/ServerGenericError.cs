using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Server0106, Direction.ServerToClient)]
public sealed partial class Server0106
{
    [UInt(8)] public byte Unknown0 { get; set; }
}
