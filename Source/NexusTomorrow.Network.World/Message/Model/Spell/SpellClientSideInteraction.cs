using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Client0805, Direction.ClientToServer)]
public sealed partial class SpellClientSideInteraction
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(3)] public byte Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
}
