using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Warns the player of their inactivity in a PvP match; the client raises MatchingPvpInactivityAlert.
/// </summary>
[WireMessage(GameMessageOpcode.Server05E6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchPvpInactivityAlert
{
    /// <summary>
    /// The time remaining, in milliseconds.
    /// </summary>
    [UInt(32)] public uint RemainingTimeMs { get; set; }
}
