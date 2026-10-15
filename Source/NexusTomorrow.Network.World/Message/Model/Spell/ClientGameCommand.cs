using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ClientGameCommand, Direction.ClientToServer)]
public sealed partial class ClientGameCommand
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }
    [UInt(4)] public byte AbilityItemIndex { get; private set; }
    [UInt(4)] public byte LimitedActionSetId { get; private set; }
    [UInt(32)] public uint TargetUnitId { get; private set; }
    public Position TargetPosition { get; private set; } = new Position();
}
