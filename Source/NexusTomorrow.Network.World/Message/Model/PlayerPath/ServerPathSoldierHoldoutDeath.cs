using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server067F, Direction.ServerToClient)]
public sealed partial class Server067F
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
