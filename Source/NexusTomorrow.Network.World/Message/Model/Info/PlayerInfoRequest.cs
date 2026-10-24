using NexusTomorrow.Game.Static.Info;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// Asks for a player's info.
/// </summary>
/// <remarks>
/// The server answers with <see cref="PlayerInfoResponse"/>, or with <see cref="PlayerInfoDetailsResponse"/> for the player's path, class,
/// level and last-online time.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0597, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class PlayerInfoRequest
{
    /// <summary>
    /// The feature asking: 0 mail; 1, 2 and 3 the friends list (2 for a friend, 3 for a rival, 1 for the others); 4 housing neighbors and
    /// a residence's owner; 5 loot and guild event logs; 8 guild event logs; 9 items; 10 matching. The client never sends 6 or 7.
    /// </summary>
    [UInt(4)] public PlayerInfoRequestType RequestType { get; private set; }

    /// <summary>
    /// The player asked about, by realm-scoped id.
    /// </summary>
    public RealmScopedId PlayerId { get; } = new();
}
