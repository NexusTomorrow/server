using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Asks the match to look for players to fill the places it has open.
/// </summary>
[WireMessage(GameMessageOpcode.Client05D5, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchLookForReplacements
{
    /// <summary>
    /// The roles to look for, as a mask of <see cref="Role"/>.
    /// </summary>
    [UInt(32)] public Role Roles { get; private set; }
}
