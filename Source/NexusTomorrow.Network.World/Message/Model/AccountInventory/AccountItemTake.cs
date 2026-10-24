using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Client0839, Direction.ClientToServer)]
public sealed partial class AccountItemTake
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
