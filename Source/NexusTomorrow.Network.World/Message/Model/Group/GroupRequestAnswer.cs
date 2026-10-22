using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Answers a request to join the group: the request GroupLib.AcceptRequest and GroupLib.DenyRequest send.
/// </summary>
[WireMessage(GameMessageOpcode.Client0453, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class GroupRequestAnswer
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    /// <summary>
    /// True from GroupLib.AcceptRequest, false from GroupLib.DenyRequest.
    /// </summary>
    [UInt(1)] public bool Accepted { get; set; }

    [WideString] public string Unknown2 { get; set; } = "";

    [UInt(1)] public bool Unknown3 { get; set; }
}
