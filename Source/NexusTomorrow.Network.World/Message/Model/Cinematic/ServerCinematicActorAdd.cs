using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Entity.Movement.Command.Mode;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server0228, Direction.ServerToClient)]
public sealed partial class Server0228
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(16)] public ushort Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    public Client0096Part0 Unknown6 { get; set; }
    [UInt(64)] public ulong Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
}
