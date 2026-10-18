using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server07F8, Direction.ServerToClient)]
public sealed partial class Server07F8
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(19)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }

    [Array(CountBits = 8)] public List<Server0262Part10> Unknown3 { get; set; } = [];
}
