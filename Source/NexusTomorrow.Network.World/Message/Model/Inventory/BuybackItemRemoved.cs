using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Removes one record from the vendor buyback list.
/// </summary>
/// <remarks>
/// The client then raises BuybackItemsUpdated; an id matching no record does nothing.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00BD, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class BuybackItemRemoved
{
    /// <summary>
    /// The record, by <see cref="Shared.Server00BAPart0.BuybackItemId"/>.
    /// </summary>
    [UInt(32)] public uint BuybackItemId { get; set; }
}
