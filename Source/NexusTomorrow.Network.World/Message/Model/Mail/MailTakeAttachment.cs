using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Takes one attachment from a mail: the request the script call MailSystemLib TakeAttachment sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0126, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MailTakeAttachment
{
    /// <summary>
    /// The mail.
    /// </summary>
    [UInt(64)] public ulong MailId { get; private set; }

    /// <summary>
    /// The attachment's position among the mail's attachments.
    /// </summary>
    [UInt(32)] public uint AttachmentIndex { get; private set; }

    /// <summary>
    /// The mailbox, as a unit id.
    /// </summary>
    [UInt(32)] public uint MailboxUnitId { get; private set; }
}
