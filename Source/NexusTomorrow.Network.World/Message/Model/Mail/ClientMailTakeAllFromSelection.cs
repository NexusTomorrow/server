using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ClientMailTakeAllFromSelection, Direction.ClientToServer)]
public sealed partial class ClientMailTakeAllFromSelection
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailList { get; } = [];
    [UInt(32)] public uint MailboxUnitId { get; private set; }
}
