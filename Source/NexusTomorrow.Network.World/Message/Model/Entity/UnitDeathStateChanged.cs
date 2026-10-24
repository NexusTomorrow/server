using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets whether a unit is dead.
/// </summary>
/// <remarks>
/// For a unit the client controls, a revival with a nonzero <see cref="Health"/> also sets the unit's health.
/// </remarks>
[WireMessage(GameMessageOpcode.Server088C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitDeathStateChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// Whether the unit is dead, as Unit:IsDead reports.
    /// </summary>
    [UInt(1)] public bool IsDead { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its handler never reads it.
    /// </summary>
    [UInt(5)] public byte Unknown2 { get; set; }

    /// <summary>
    /// The health a unit the client controls takes on revival. The client applies it only when it is nonzero and the unit is alive and
    /// controlled, through the unit's health setter, whose value Unit:GetHealth reports.
    /// </summary>
    [UInt(32)] public uint Health { get; set; }
}
