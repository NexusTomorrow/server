using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Changes one of the player's neighbors.
/// </summary>
[WireMessage(GameMessageOpcode.Server0519, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingNeighborUpdate
{
    /// <summary>
    /// The neighbor.
    /// </summary>
    public HousingNeighbor Neighbor { get; set; } = new();

    [UInt(3)] public byte Unknown1 { get; set; }
}
