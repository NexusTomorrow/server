using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Removes one attachment from a mail the client holds when <see cref="Result"/> is 0, and marks the mail read and not
/// returnable.
/// </summary>
/// <remarks>
/// Any other result the client raises as its MailResult event, changing nothing.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05A8, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MailAttachmentResult
{
    /// <summary>
    /// The mail.
    /// </summary>
    [UInt(64)] public ulong MailId { get; set; }

    /// <summary>
    /// 0 to remove the attachment; any other value is a GameLib.CodeEnumGenericError code.
    /// </summary>
    [UInt(8)] public byte Result { get; set; }

    /// <summary>
    /// The attachment's position among the mail's attachments; the client ignores a position past the last.
    /// </summary>
    [UInt(32)] public uint AttachmentIndex { get; set; }
}
