using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Client0125, Direction.ClientToServer)]
public sealed partial class MailTakeAllAttachments
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> Unknown0 { get; } = [];
    [UInt(32)] public uint Unknown1 { get; private set; }
}
