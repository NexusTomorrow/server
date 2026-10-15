using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellThresholdClear, Direction.ServerToClient)]
public sealed partial class ServerSpellThresholdClear
{
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(1)] public bool ProcessCooldown { get; set; }
}
