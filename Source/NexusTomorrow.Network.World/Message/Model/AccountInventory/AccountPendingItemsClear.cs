using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Clears every pending item, grouped and ungrouped; the client raises no event.
/// </summary>
[WireMessage(GameMessageOpcode.Server0977, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountPendingItemsClear
{
}
