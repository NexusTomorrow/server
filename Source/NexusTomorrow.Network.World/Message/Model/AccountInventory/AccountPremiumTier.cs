using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server097F, Direction.ServerToClient)]
public sealed partial class AccountPremiumTier
{
    [UInt(5)] public byte Unknown0 { get; set; }
}
