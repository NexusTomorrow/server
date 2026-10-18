using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Trade;

[WireMessage(GameMessageOpcode.Client018D, Direction.ClientToServer)]
public sealed partial class Client018D
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
