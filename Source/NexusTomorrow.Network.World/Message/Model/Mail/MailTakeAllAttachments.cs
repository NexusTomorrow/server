using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Takes all attachments from one or more mails: the request the script calls MailSystemLib TakeAllAttachments and
/// TakeAllAttachmentsFromMultipleMessages send.
/// </summary>
/// <remarks>
/// The client sends only mails it holds, each once.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0125, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MailTakeAllAttachments
{
    /// <summary>
    /// The mails, by id.
    /// </summary>
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailIds { get; } = [];

    /// <summary>
    /// The mailbox, as a unit id.
    /// </summary>
    [UInt(32)] public uint MailboxUnitId { get; private set; }
}
