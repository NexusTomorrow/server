using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellInterrupted2, Direction.ServerToClient)]
public sealed partial class ServerSpellInterrupted2
{
    [UInt(32)] public uint CastingId { get; set; }
    [UInt(9)] public CastResult Spell4CastResultId { get; set; }
    [UInt(32)] public uint CasterUnitId { get; set; }
}

