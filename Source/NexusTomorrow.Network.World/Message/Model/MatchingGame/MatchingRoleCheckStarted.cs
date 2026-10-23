using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Starts a role check; the client marks a role check active and raises MatchingRoleCheckStarted.
/// </summary>
[WireMessage(GameMessageOpcode.Server05F1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchingRoleCheckStarted
{
    /// <summary>
    /// Whether the player must choose roles to answer the check.
    /// </summary>
    [UInt(1)] public bool RolesRequired { get; set; }
}
