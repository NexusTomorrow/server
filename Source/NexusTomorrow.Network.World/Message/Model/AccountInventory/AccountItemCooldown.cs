using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireStruct]
public sealed partial class AccountItemCooldown
{
    [UInt(32)] public uint AccountItemCooldownGroup { get; set; }
    [UInt(32)] public uint CooldownInSeconds { get; set; }
}
