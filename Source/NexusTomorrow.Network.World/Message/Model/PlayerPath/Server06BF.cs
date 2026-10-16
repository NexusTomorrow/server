using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06BF, Direction.ServerToClient)]
public sealed partial class Server06BF
{
    [UInt(15)] public ushort Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
}
