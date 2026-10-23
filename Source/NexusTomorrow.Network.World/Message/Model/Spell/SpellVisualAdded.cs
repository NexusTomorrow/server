using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0263, Direction.ServerToClient)]
public sealed partial class SpellVisualAdded
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(17)] public uint Unknown2 { get; set; }
    [UInt(17)] public uint Unknown3 { get; set; }
    [UInt(17)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    public Client0096Part0 Unknown6 { get; set; } = new();
}
