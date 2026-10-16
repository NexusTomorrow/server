using NexusTomorrow.Game.Static.Loot;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Server08A6, Direction.ServerToClient)]
public sealed partial class Server08A6
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(64)] public ulong Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
}
