using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server07FA, Direction.ServerToClient)]
public sealed partial class Server07FA
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(9)] public ushort Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}

