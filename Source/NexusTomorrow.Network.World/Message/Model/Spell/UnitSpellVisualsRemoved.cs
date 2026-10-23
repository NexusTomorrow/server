using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Removes every spell visual on a unit.
/// </summary>
[WireMessage(GameMessageOpcode.Server0785, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitSpellVisualsRemoved
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}

