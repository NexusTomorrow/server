using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server096D, Direction.ServerToClient)]
public sealed partial class AccountItems
{
    [Array(CountBits = 32)] public List<AccountItem> Unknown0 { get; set; } = [];
}
