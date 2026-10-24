using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Server04DA, Direction.ServerToClient)]
public sealed partial class HealthyGamingUpdate
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
}
