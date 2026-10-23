using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Asks to join a player's group: the request GroupLib.Join sends.
/// </summary>
/// <remarks>
/// The shipped add-ons call GroupLib.Join with the player's display name and the realm name.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0422, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupJoin
{
    /// <summary>
    /// The player whose group to join.
    /// </summary>
    [WideString] public string PlayerName { get; set; } = "";

    /// <summary>
    /// That player's realm, empty when the call gives none.
    /// </summary>
    [WideString] public string RealmName { get; set; } = "";
}
