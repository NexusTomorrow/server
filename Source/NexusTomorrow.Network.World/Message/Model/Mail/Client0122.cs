using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Client0122, Direction.ClientToServer)]
public sealed partial class Client0122
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> Unknown0 { get; } = [];
}
