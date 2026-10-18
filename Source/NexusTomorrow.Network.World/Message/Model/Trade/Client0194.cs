using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Trade;

[WireMessage(GameMessageOpcode.Client0194, Direction.ClientToServer)]
public sealed partial class Client0194
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
