using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Client0802, Direction.ClientToServer)]
public sealed partial class SpellCancel
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
