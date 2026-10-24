using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server096C, Direction.ServerToClient)]
public sealed partial class AccountItemInProgressRemove
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
}
