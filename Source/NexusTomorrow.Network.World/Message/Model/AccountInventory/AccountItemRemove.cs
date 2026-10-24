using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server097C, Direction.ServerToClient)]
public sealed partial class AccountItemRemove
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
