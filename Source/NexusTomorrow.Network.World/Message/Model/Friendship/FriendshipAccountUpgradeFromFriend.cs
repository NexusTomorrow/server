using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Asks to make a character friend an account friend.
/// </summary>
[WireMessage(GameMessageOpcode.Client0398, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountUpgradeFromFriend
{
    /// <summary>
    /// The friend, by <see cref="FriendshipCharacter.CharacterId"/>.
    /// </summary>
    public RealmScopedId CharacterId { get; private set; } = new();

    /// <summary>
    /// The note that goes with the request.
    /// </summary>
    [WideString] public string Note { get; private set; } = "";
}
