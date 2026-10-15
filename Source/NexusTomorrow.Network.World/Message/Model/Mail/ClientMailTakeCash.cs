using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ClientMailTakeCash, Direction.ClientToServer)]
public sealed partial class ClientMailTakeCash
{
    [UInt(64)] public ulong MailId { get; private set; }
    [UInt(32)] public uint MailboxUnitId { get; private set; } // Mailbox being interacted with
}
