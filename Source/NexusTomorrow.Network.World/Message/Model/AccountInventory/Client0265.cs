using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.Client0265, Direction.ClientToServer)]
public sealed partial class Client0265
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
