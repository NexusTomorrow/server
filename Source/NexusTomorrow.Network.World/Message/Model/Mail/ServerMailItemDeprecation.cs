using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ServerMailItemDeprecation, Direction.ServerToClient)]
public sealed partial class ServerMailItemDeprecation
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailIds { get; set; } = [];
}
