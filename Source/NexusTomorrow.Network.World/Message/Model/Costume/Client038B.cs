using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Client038B, Direction.ClientToServer)]
public sealed partial class Client038B
{
    [UInt(18)] public uint Unknown0 { get; private set; }
}
