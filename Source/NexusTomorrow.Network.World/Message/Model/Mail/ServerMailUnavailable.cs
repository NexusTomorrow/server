using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ServerMailUnavailable, Direction.ServerToClient)]
public sealed partial class ServerMailUnavailable
{
    [UInt(64)] public ulong MailId { get; set; }
}
