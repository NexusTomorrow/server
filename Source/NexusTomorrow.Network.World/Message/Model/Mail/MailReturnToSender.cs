using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Client0124, Direction.ClientToServer)]
public sealed partial class MailReturnToSender
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
