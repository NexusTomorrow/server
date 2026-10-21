using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Takes a mail's money: the request the script call MailSystemLib TakeMoney sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0127, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MailTakeMoney
{
    /// <summary>
    /// The mail.
    /// </summary>
    [UInt(64)] public ulong MailId { get; private set; }

    /// <summary>
    /// The mailbox, as a unit id.
    /// </summary>
    [UInt(32)] public uint MailboxUnitId { get; private set; }
}
