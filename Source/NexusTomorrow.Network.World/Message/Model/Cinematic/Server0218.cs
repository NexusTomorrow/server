using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server0218, Direction.ServerToClient)]
public sealed partial class Server0218
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(16)] public ushort Unknown3 { get; set; }
    [UInt(16)] public ushort Unknown4 { get; set; }
    [UInt(16)] public ushort Unknown5 { get; set; }
}
