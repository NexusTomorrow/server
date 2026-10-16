using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server0216, Direction.ServerToClient)]
public sealed partial class Server0216
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(16)] public ushort Unknown2 { get; set; }
    [UInt(16)] public ushort Unknown3 { get; set; }
    [UInt(16)] public ushort Unknown4 { get; set; }
}
