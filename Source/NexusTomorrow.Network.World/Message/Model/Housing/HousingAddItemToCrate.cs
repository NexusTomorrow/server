using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Puts an inventory item into the housing crate: the request HousingLib.AddItemToCrate sends.
/// </summary>
/// <remarks>
/// The client sends it only for an item with a HousingDecorInfo row.
/// </remarks>
[WireMessage(GameMessageOpcode.Client04E7, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingAddItemToCrate
{
    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; private set; }
}
