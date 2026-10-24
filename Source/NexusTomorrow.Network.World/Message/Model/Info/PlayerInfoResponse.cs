using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// Answers <see cref="PlayerInfoRequest"/> with a player's name and faction.
/// </summary>
/// <remarks>
/// Every name lookup for the player takes its answer from this message or from <see cref="PlayerInfoDetailsResponse"/>. Of the detail
/// requests waiting on the player, the client hands the player's info only to those of type 0, 1, 5 and 8; the others take it from
/// <see cref="PlayerInfoDetailsResponse"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0598, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerInfoResponse
{
    /// <summary>
    /// The player's id, name and faction.
    /// </summary>
    public PlayerInfoSummary Summary { get; set; } = new();
}
