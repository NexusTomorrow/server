using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// Sets the world's time of day and the length of its day.
/// </summary>
/// <remarks>
/// The client advances the time of day between updates and wraps it at 86400 seconds; GameLib.GetWorldTimeOfDay returns it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0845, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class TimeOfDayChanged
{
    /// <summary>
    /// The world's time of day in seconds, which the client advances between updates.
    /// </summary>
    [UInt(32)] public uint TimeOfDaySeconds { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It stores it but never reads it.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The length of a day in seconds. The client advances the time of day at 86400 divided by it, so it is never 0.
    /// </summary>
    [UInt(32)] public uint DayLengthSeconds { get; set; }
}
