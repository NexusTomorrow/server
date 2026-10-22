using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Replaces the vendor buyback list with these records.
/// </summary>
/// <remarks>
/// The client raises BuybackItemsUpdated once every record is stored.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00BC, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class BuybackItems
{
    [Array(CountBits = 32)] public List<Server00BAPart0> Unknown0 { get; } = [];
}
