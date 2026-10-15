using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Fires at regular intervals if the player is AFK in a PvP match.
// This is triggered when the player has 2 minutes, 1 minute, 30 seconds, and 5 second intervals before they are removed from a match
[WireMessage(GameMessageOpcode.ServerMatchingPvpInactivityAlert, Direction.ServerToClient)]
public sealed partial class ServerMatchingPvpInactivityAlert
{
    [UInt(32)] public uint RemainingTimeMs { get; set; }
}
