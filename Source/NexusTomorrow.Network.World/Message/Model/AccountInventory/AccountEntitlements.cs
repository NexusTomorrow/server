using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0968, Direction.ServerToClient)]
public sealed partial class AccountEntitlements
{
    [WireStruct]
    public sealed partial class AccountEntitlement
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [Array(CountBits = 32)] public List<AccountEntitlement> Unknown0 { get; set; } = [];
}
