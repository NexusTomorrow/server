using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// The result of a mail action on one mail.
/// </summary>
/// <remarks>
/// Any result but 0 the client raises as its MailResult event. On 0 with <see cref="Unknown0"/> 2 or 3, for a mail it holds, it clears
/// the mail's gift and CoD amounts and marks it read and not returnable.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05A2, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MailResult
{
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The mail.
    /// </summary>
    [UInt(64)] public ulong MailId { get; set; }

    /// <summary>
    /// 0 when the action succeeded; any other value is a GameLib.CodeEnumGenericError code.
    /// </summary>
    [UInt(8)] public byte Result { get; set; }
}
