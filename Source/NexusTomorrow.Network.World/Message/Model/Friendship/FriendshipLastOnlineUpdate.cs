using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets when a friendship's character was last online.
/// </summary>
/// <remarks>
/// The client stores the time and then asks for the character's player info (<see cref="Info.Client0597"/>); the answer raises
/// FriendshipUpdate and FriendshipUpdateOnline. Any answer to that request overwrites the friend's level and last-online time, so
/// this time must agree with what the server answers there.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03CA, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipLastOnlineUpdate
{
    /// <summary>
    /// The friendship, by <see cref="FriendshipCharacter.CharacterId"/>.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// When the friend was last online, in days from now, which add-ons read as fLastOnline:
    /// 0 clears it and a negative value is that many days ago.
    /// </summary>
    [Float32] public float LastOnlineDays { get; set; }
}
