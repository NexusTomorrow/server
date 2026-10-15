using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ClientMailPayCod, Direction.ClientToServer)]
public sealed partial class ClientMailPayCod
{
    [UInt(64)] public ulong MailId { get; private set; }
    [UInt(32)] public uint MailboxUnitId { get; private set; } // Must be interacting with a mailbox to pay COD
}
