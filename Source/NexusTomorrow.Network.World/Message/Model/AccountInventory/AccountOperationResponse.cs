using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0970, Direction.ServerToClient)]
public sealed partial class AccountOperationResponse
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
