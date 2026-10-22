using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Raises the client's RepairItemCompleted event.
/// </summary>
[WireMessage(GameMessageOpcode.Server014D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class RepairItemCompleted
{
}
