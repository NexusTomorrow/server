using NexusTomorrow.Game.Static.Info;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// Sets the player's healthy gaming state: whether it is enforced, the status and the time played.
/// </summary>
/// <remarks>
/// GameLib.GetHealthyGamingStatus returns the state. The client notifies the player on a timer whose period <see cref="Status"/> chooses,
/// and raises HealthyGamingUpdate with its notices.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04DA, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HealthyGamingUpdate
{
    /// <summary>
    /// Whether healthy gaming is enforced. The client stores the state either way, and acts on it only while this is set.
    /// </summary>
    [UInt(1)] public bool Enforced { get; set; }

    /// <summary>
    /// The player's healthy gaming status.
    /// </summary>
    [UInt(2)] public HealthyGamingStatus Status { get; set; }

    /// <summary>
    /// The time the played time counts from, as a Windows FILETIME.
    /// </summary>
    [UInt(64)] public ulong SessionStartTime { get; set; }

    /// <summary>
    /// Seconds played before <see cref="SessionStartTime"/>, which the client adds to the time since it.
    /// </summary>
    [UInt(32)] public uint PriorPlayedSeconds { get; set; }

    [UInt(32)] public uint Unknown4 { get; set; }
}
