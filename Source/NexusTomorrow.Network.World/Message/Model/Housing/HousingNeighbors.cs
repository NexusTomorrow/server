using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Lists the player's neighbors.
/// </summary>
/// <remarks>
/// The client skips a record for the player's own character.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0507, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingNeighbors
{
    /// <summary>
    /// The neighbors.
    /// </summary>
    [Array(CountBits = 32)] public List<HousingNeighbor> Neighbors { get; set; } = [];
}
