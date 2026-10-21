using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Pays a mail's CoD: the request the script call MailSystemLib PayCoD sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0123, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MailPayCod
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
