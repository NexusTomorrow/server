using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server096A, Direction.ServerToClient)]
public sealed partial class AccountItemInProgressAdd
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public AccountItem Unknown1 { get; set; } = new();
}
