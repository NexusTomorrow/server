using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server0681, Direction.ServerToClient)]
public sealed partial class Server0681
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
}
