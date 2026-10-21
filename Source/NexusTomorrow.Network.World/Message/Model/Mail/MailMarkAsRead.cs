using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Marks mails read: the request the script calls MailSystemLib MarkAsRead and MarkMultipleMessagesAsRead send.
/// </summary>
/// <remarks>
/// The client marks the mails read itself as it sends, and sends only mails it holds that were unread, each once.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0122, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MailMarkAsRead
{
    /// <summary>
    /// The mails, by id.
    /// </summary>
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailIds { get; } = [];
}
