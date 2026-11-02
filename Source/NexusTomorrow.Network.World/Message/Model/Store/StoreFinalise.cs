using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Store;

/// <summary>
/// Finalises the store, processing its pending entries; the client carries no fields.
/// </summary>
[WireMessage(GameMessageOpcode.Server0987, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class StoreFinalise
{
}
