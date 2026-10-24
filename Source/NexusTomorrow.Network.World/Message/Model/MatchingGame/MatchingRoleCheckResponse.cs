using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// The player's answer to a role check: the roles chosen, or a refusal.
/// </summary>
[WireMessage(GameMessageOpcode.Client05B2, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class MatchingRoleCheckResponse
{
    [UInt(5)] public byte Unknown0 { get; private set; }

    /// <summary>
    /// The roles the player chose, as a mask of <see cref="Role"/>, limited to the roles the player's class can fill.
    /// </summary>
    [UInt(32)] public Role Roles { get; private set; }

    /// <summary>
    /// Whether the player confirmed the role check (true) or declined it (false).
    /// </summary>
    [UInt(1)] public bool Accepted { get; private set; }
}
