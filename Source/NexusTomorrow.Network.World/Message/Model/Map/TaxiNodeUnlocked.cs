using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Unlocks a flight path node for the player; the client raises FlightPathUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server0186, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class TaxiNodeUnlocked
{
    /// <summary>
    /// The node, as a TaxiNode id, which the client adds to the nodes the player knows.
    /// </summary>
    [UInt(14)] public ushort TaxiNodeId { get; set; }
}
