using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountEntitlements, Direction.ServerToClient)]
public sealed partial class ServerAccountEntitlements
{
    [WireStruct]
    public sealed partial class AccountEntitlement
    {
        [UInt(32)] public EntitlementType EntitlementId { get; set; }
        [UInt(32)] public uint Count { get; set; }
    }

    [Array(CountBits = 32)] public List<AccountEntitlement> AccountEntitlements { get; set; } = [];
}
