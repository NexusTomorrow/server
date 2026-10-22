using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Answers a group invitation: the request GroupLib.AcceptInvite and GroupLib.DeclineInvite send.
/// </summary>
[WireMessage(GameMessageOpcode.Client041A, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class GroupInviteAnswer
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    /// <summary>
    /// True from GroupLib.AcceptInvite, false from GroupLib.DeclineInvite.
    /// </summary>
    [UInt(1)] public bool Accepted { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
}
