using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Removes one mail from the client's mailbox.
/// </summary>
/// <remarks>
/// The client then raises its UnavailableMail event with the mail's id.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05A7, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnavailableMail
{
    /// <summary>
    /// The mail.
    /// </summary>
    [UInt(64)] public ulong MailId { get; set; }
}
