using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0975, Direction.ServerToClient)]
public sealed partial class AccountItemAdd
{
    public AccountItem Unknown0 { get; set; } = new();
}
