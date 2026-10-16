using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server07F9, Direction.ServerToClient)]
public sealed partial class Server07F9
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(9)] public ushort Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
}
