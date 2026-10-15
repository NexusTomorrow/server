using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellBuffRemove, Direction.ServerToClient)]
public sealed partial class ServerSpellBuffRemove
{
    [UInt(32)] public uint ServerUniqueId { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Targets { get; set; } = [];
}
