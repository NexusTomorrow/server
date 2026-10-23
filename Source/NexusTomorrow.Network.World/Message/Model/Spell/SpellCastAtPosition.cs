using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Client009B, Direction.ClientToServer)]
public sealed partial class SpellCastAtPosition
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(16)] public ushort Unknown1 { get; private set; }
    public Client0096Part0 Unknown2 { get; private set; } = new Client0096Part0();
}
