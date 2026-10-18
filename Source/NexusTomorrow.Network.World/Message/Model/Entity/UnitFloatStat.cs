using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets one float stat on a unit, replacing its value.
/// </summary>
/// <remarks>
/// For the player's dash energy (stat 9) or endurance (stat 2) the client also tells the add-ons. <see cref="UnitIntegerStat"/> is the
/// same message with an integer value.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0935, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitFloatStat
{
    /// <summary>
    /// The unit's id. The client drops the message when it has no unit with this id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The stat and its new value.
    /// </summary>
    public UnitFloatStatValue Stat { get; set; } = new();
}
