using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellAuraCancel, Direction.ServerToClient)]
public sealed partial class ServerSpellAuraCancel
{
    [UInt(32)] public uint TargetUnitId { get; set; }
    [UInt(32)] public uint ServerUniqueId { get; set; }
}
