using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the player's own friendship status, which FriendshipLib.GetPersonalStatus returns.
/// </summary>
[WireMessage(GameMessageOpcode.Server03A6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipPersonalStatus
{
    /// <summary>
    /// The player's public note, which GetPersonalStatus returns as strPublicNote.
    /// </summary>
    [WideString] public string PublicNote { get; set; } = "";

    /// <summary>
    /// The player's display name, which GetPersonalStatus returns as strDisplayName.
    /// </summary>
    [WideString] public string DisplayName { get; set; } = "";

    /// <summary>
    /// The player's presence.
    /// </summary>
    [UInt(3)] public AccountPresenceState Presence { get; set; }

    /// <summary>
    /// Whether the player ignores invites from strangers, which GetPersonalStatus returns as bIgnoreStrangerInvites.
    /// </summary>
    /// <remarks>
    /// The client stores this value whole, so any nonzero value means on; <see cref="FriendshipIgnoreStrangersState.Flags"/> keeps only
    /// one bit.
    /// </remarks>
    [UInt(1)] public bool IgnoreStrangers { get; set; }
}
