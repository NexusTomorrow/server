using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Starts a ready check in the player's current group.
/// </summary>
/// <remarks>
/// The client raises Group_ReadyCheck and starts a timer of GameFormula row 1128's value (60000 when the row is missing).
/// </remarks>
[WireMessage(GameMessageOpcode.Server044C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupReadyCheck
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The member the event names as nMemberIdx.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    /// <summary>
    /// The text the event passes as strMessage.
    /// </summary>
    [WideString] public string Message { get; set; } = "";
}
