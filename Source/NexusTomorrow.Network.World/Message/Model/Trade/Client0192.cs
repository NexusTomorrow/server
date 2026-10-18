using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Trade;

[WireMessage(GameMessageOpcode.Client0192, Direction.ClientToServer)]
public sealed partial class Client0192
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
