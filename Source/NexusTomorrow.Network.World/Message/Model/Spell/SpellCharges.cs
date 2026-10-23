using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0914, Direction.ServerToClient)]
public sealed partial class SpellCharges
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}

