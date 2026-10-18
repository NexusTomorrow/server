using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server05A0, Direction.ServerToClient)]
public sealed partial class Server05A0
{
    [UInt(9)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
