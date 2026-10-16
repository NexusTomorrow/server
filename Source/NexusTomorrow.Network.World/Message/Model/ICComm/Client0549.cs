using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.Client0549, Direction.ClientToServer)]
public sealed partial class Client0549
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
