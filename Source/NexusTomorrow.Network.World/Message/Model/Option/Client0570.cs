using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.Client0570, Direction.ClientToServer)]
public sealed partial class Client0570
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
