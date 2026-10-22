using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Stores one record in the vendor buyback list.
/// </summary>
/// <remarks>
/// The client raises BuybackItemsUpdated once the record is stored.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00BA, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class BuybackItemUpdated
{
    public Server00BAPart0 Unknown0 { get; set; } = new();
}
