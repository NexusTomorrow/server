using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ClientMoveToCast, Direction.ClientToServer)]
public sealed partial class ClientMoveToCast
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }
    [UInt(16)] public ushort BagIndex { get; private set; }
    public Position MoveToPosition { get; private set; } = new Position();
}
