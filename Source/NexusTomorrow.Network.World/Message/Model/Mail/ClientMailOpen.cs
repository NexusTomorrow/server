using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ClientMailOpen, Direction.ClientToServer)]
public sealed partial class ClientMailOpen
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailList { get; } = [];
}
