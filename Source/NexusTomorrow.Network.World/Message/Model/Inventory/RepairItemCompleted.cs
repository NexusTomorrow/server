using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server014D, Direction.ServerToClient)]
public sealed partial class RepairItemCompleted
{
}
