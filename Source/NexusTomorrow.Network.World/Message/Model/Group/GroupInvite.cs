using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Invites a player to the group: the request GroupLib.Invite and Unit:InviteToGroup send.
/// </summary>
/// <remarks>
/// GroupLib.Invite's third argument, strMessage, is not sent.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0416, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupInvite
{
    /// <summary>
    /// The player: GroupLib.Invite's strPlayerName, or the unit's name.
    /// </summary>
    [WideString] public string PlayerName { get; set; } = "";

    /// <summary>
    /// GroupLib.Invite's strRealmName; Unit:InviteToGroup sends it empty.
    /// </summary>
    [WideString] public string RealmName { get; set; } = "";
}
