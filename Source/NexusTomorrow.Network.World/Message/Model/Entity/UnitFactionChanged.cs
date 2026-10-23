using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets a unit's faction.
/// </summary>
[WireMessage(GameMessageOpcode.Server0934, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitFactionChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The faction, a Faction2 table id.
    /// </summary>
    [UInt(14)] public ushort FactionId { get; set; }
}
