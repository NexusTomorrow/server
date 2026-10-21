using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Deletes mails: the request the script calls MailSystemLib DeleteMessage and DeleteMultipleMessages send.
/// </summary>
/// <remarks>
/// The client sends only mails it holds, each once.
/// </remarks>
[WireMessage(GameMessageOpcode.Client011E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MailDeleteMessages
{
    /// <summary>
    /// The mails, by id.
    /// </summary>
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailIds { get; } = [];
}
