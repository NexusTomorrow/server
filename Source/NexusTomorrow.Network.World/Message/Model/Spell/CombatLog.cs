using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0247, Direction.ServerToClient)]
public partial class CombatLog
{
    [UInt(6)] public byte Unknown0 { get; set; }
    [Union(nameof(Unknown0))] public ICombatLogPayload Unknown1 { get; set; } = new CombatLogAmount();
}
