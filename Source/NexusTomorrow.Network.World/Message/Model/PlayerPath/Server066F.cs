using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server066F, Direction.ServerToClient)]
public sealed partial class Server066F
{
    [UInt(14)] public uint Unknown0 { get; set; }
}
