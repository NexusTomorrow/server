using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server080F, Direction.ServerToClient)]
public sealed partial class SpellTargetsAdded
{
    [WireStruct]
    public sealed partial class Server080FPart0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
    };

    [Array(CountBits = 32)] public List<Server080FPart0> Unknown0 { get; set; } = [];
}
