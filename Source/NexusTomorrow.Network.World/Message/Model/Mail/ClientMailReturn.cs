using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ClientMailReturn, Direction.ClientToServer)]
public sealed partial class ClientMailReturn
{
    [UInt(64)] public ulong MailId { get; private set; }
}
