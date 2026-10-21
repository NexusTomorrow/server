using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Client011E, Direction.ClientToServer)]
public sealed partial class MailDeleteMessages
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> Unknown0 { get; } = [];
}
