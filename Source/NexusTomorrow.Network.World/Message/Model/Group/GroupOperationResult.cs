using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Reports the result of a group operation.
/// </summary>
/// <remarks>
/// For its current group the client raises Group_Operation_Result with the member's name and the result.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0443, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupOperationResult
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The member the operation concerned.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    /// <summary>
    /// A GroupLib.ActionResult value, the event's eResult.
    /// </summary>
    [UInt(6)] public byte Result { get; set; }
}
