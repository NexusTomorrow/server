using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets one integer stat on a unit, replacing its value.
/// </summary>
/// <remarks>
/// A float stat sent here is set to 0. <see cref="UnitFloatStat"/> is the same message with a float value.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0938, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitIntegerStat
{
    /// <summary>
    /// A stat and the integer value that replaces it.
    /// </summary>
    [WireStruct]
    public sealed partial class UnitIntegerStatValue
    {
        /// <summary>
        /// The stat, 0 to 31. The client keeps stats 10, 11, 15, 17, 20, 21, 22 and 26 as integers, and a float stat sent here is set to 0.
        /// Stat 10 is the level, and setting it raises PlayerLevelChange; setting stat 20 refreshes the health and shield bar
        /// (RefreshHealthShieldBar); stat 21 is the interrupt armor.
        /// </summary>
        [UInt(5)] public byte Id { get; set; }

        /// <summary>
        /// The stat's new value, which replaces the old one.
        /// </summary>
        [UInt(32)] public uint Value { get; set; }
    }

    /// <summary>
    /// The unit's id. The client drops the message when it has no unit with this id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The stat and its new value.
    /// </summary>
    public UnitIntegerStatValue Stat { get; set; } = new();
}
