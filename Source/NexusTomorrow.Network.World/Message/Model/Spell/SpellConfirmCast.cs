using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server07FD, Direction.ServerToClient)]
public sealed partial class SpellConfirmCast
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    public Client0096Part0 Unknown3 { get; set; } = new Client0096Part0();
    [Float32] public float Unknown4 { get; set; }

    [Array(CountBits = 8)] public List<Server0262Part12> Unknown5 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server0262Part13> Unknown6 { get; set; } = [];
}
