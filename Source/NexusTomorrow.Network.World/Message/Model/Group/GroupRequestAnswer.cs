using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Answers a request to join the group: the request GroupLib.AcceptRequest and GroupLib.DenyRequest send.
/// </summary>
/// <remarks>
/// It answers the pending request the client keeps from the last <see cref="GroupJoinRequest"/> or <see cref="GroupReferral"/>,
/// whose other fields it sends back unchanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0453, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupRequestAnswer
{
    /// <summary>
    /// The request's id, as the request gave it.
    /// </summary>
    [UInt(64)] public ulong RequestId { get; set; }

    /// <summary>
    /// True from GroupLib.AcceptRequest, false from GroupLib.DenyRequest.
    /// </summary>
    [UInt(1)] public bool Accepted { get; set; }

    /// <summary>
    /// The name the request carried: the player asking to join, or the player referred.
    /// </summary>
    [WideString] public string RequesterName { get; set; } = "";

    /// <summary>
    /// True when it answers a <see cref="GroupJoinRequest"/>, false when it answers a <see cref="GroupReferral"/>.
    /// </summary>
    [UInt(1)] public bool IsJoinRequest { get; set; }
}
