using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellInterrupted1, Direction.ServerToClient)]
public sealed partial class ServerSpellInterrupted1
{
    [UInt(32)] public uint ServerUniqueId { get; set; }
    [UInt(9)] public CastResult CastResult { get; set; }
    [UInt(32)] public uint CasterUnitId { get; set; }
    [UInt(1)] public bool CancelCast { get; set; }
}
