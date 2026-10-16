using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[WireMessage(GameMessageOpcode.Client00B9, Direction.ClientToServer)]
public sealed partial class Client00B9
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
