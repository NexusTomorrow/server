using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Client0801, Direction.ClientToServer)]
public sealed partial class SpellCastStop
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(9)] public ushort Unknown1 { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
}
