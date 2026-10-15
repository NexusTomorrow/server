using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.ServerEntityCCStateRemove, Direction.ServerToClient)]
public sealed partial class ServerEntityCCStateRemove
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(5)] public CCState CCType { get; set; }
    [UInt(32)] public uint SpellCastUniqueId { get; set; } // Must match the SpellCastUniqueId from ServerSpellGo/ServerSpellExecute
    [UInt(32)] public uint SpellEffectUniqueId { get; set; } // Must match the SpellEffectUniqueId from ServerSpellGo/ServerSpellExecute. TBC can be 0 for some CCs
    [UInt(1)] public bool Removed { get; set; }
}
