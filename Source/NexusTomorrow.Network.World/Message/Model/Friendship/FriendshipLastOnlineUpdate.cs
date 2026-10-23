using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets when a friendship's character was last online.
/// </summary>
/// <remarks>
/// The client raises FriendshipUpdate.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03CA, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class FriendshipLastOnlineUpdate
{
    /// <summary>
    /// The friendship, by <see cref="Friendship.CharacterId"/>.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// When the character was last online, which add-ons read as fLastOnline; 0 clears it.
    /// </summary>
    [Float32] public float LastOnline { get; set; }
}
