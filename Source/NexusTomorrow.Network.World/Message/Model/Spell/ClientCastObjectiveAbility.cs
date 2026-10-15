using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ClientCastObjectiveAbility, Direction.ClientToServer)]
public sealed partial class ClientCastObjectiveAbility
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }
    [UInt(18)] public uint Spell4Id { get; private set; }
    [UInt(32)] public uint TargetUnitId { get; private set; }
    public Position Position { get; private set; } = new Position();
}
