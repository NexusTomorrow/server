using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellBuffRemoveSingle, Direction.ServerToClient)]
public sealed partial class ServerSpellBuffRemoveSingle
{
    [UInt(32)] public uint ServerUniqueId { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }
}
