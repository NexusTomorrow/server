using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Mails for which the client raises its ItemDeprecationAlert event, passing the mail.
/// </summary>
/// <remarks>
/// For a listed mail it holds the client raises the event at once; it keeps the other ids and raises the event when that mail
/// arrives.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0566, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ItemDeprecationAlert
{
    /// <summary>
    /// The mails, by id.
    /// </summary>
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailIds { get; set; } = [];
}
