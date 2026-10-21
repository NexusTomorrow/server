using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Returns a mail to its sender: the request the script call MailSystemLib ReturnToSender sends.
/// </summary>
/// <remarks>
/// The client sends it only for a mail it holds whose flags lack 0x8; otherwise it raises
/// GameLib.CodeEnumGenericError.Mail_DoesNotExist or Mail_CannotReturn as its MailResult event.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0124, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MailReturnToSender
{
    /// <summary>
    /// The mail.
    /// </summary>
    [UInt(64)] public ulong MailId { get; private set; }
}
