using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Contract;

[WireMessage(GameMessageOpcode.Server0361, Direction.ServerToClient)]
public sealed partial class Server0361
{
    [UInt(15)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
