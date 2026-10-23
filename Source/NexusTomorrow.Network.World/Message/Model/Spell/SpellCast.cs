using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Client0096, Direction.ClientToServer)]
public sealed partial class SpellCast
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(4)] public byte Unknown1 { get; private set; }
    [UInt(4)] public byte Unknown2 { get; private set; }
    [UInt(32)] public uint Unknown3 { get; private set; }
    public Client0096Part0 Unknown4 { get; private set; } = new Client0096Part0();
}
