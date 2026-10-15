using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellCastResult, Direction.ServerToClient)]
public sealed partial class ServerSpellCastResult
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; set; }
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(9)] public CastResult CastResult { get; set; }
}
