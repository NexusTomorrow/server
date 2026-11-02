using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Store;

/// <summary>
/// Marks the storefront as updated and raises StoreCatalogUpdated; the client carries no fields.
/// </summary>
[WireMessage(GameMessageOpcode.Server0989, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class StoreCatalogUpdated
{
}
