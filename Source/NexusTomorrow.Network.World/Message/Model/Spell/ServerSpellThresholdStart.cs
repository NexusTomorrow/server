using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellThresholdStart, Direction.ServerToClient)]
public sealed partial class ServerSpellThresholdStart
{
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(18)] public uint RootSpell4Id { get; set; }
    [UInt(18)] public uint ParentSpell4Id { get; set; } = 0;
    [UInt(32)] public uint ServerUniqueId { get; set; }
}
