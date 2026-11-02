using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// The result of a mail action on one mail.
/// </summary>
/// <remarks>
/// Any result but 0 the client raises as its MailResult event. On 0 with <see cref="Action"/> 2 or 3, for a mail it holds, it clears
/// the mail's gift and CoD amounts and marks it read and not returnable.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05A2, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MailResult
{
    /// <summary>
    /// The mail action this result is for; 2 or 3 clears the mail's gift and CoD amounts and marks it read.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Action { get; set; }

    /// <summary>
    /// The mail.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong MailId { get; set; }

    /// <summary>
    /// 0 when the action succeeded; any other value is a GameLib.CodeEnumGenericError code.
    /// </summary>
    [VerifiedMember, UInt(8)] public byte Result { get; set; }
}
