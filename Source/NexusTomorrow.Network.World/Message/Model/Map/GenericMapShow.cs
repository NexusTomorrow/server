using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Shows a generic map with the states of its nodes.
/// </summary>
[WireMessage(GameMessageOpcode.Server0758, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GenericMapShow
{
    /// <summary>
    /// The map, as a GenericMap id.
    /// </summary>
    [UInt(14)] public uint GenericMapId { get; set; }

    /// <summary>
    /// The nodes and their states; the client shows only the nodes whose GenericMapNode row belongs to <see cref="GenericMapId"/>.
    /// </summary>
    [Array(CountBits = 32)] public List<GenericMapNodeState> Nodes { get; set; } = [];
}
