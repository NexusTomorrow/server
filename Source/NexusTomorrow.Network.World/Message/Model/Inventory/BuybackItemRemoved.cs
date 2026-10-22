using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server00BD, Direction.ServerToClient)]
public sealed partial class BuybackItemRemoved
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
