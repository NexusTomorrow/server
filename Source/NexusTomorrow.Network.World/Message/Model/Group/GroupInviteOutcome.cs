using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Reports what became of a group invitation.
/// </summary>
/// <remarks>
/// The client raises Group_Invite_Result with the name and the result.
/// </remarks>
[WireMessage(GameMessageOpcode.Server041D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupInviteOutcome
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(64)] public ulong Unknown0 { get; set; }

    /// <summary>
    /// The player the invitation concerns, by name.
    /// </summary>
    [WideString] public string PlayerName { get; set; } = "";

    /// <summary>
    /// A GroupLib.Result value. On ExpiredInvitee (10) the client also drops the invitation it holds.
    /// </summary>
    [UInt(5)] public byte Result { get; set; }
}
