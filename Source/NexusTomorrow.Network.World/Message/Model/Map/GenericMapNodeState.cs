using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// A node of a generic map and whether it is enabled.
/// </summary>
[WireStruct]
public sealed partial class GenericMapNodeState
{
    /// <summary>
    /// The node, as a GenericMapNode id.
    /// </summary>
    [UInt(14)] public uint GenericMapNodeId { get; set; }

    /// <summary>
    /// Whether the node is enabled.
    /// </summary>
    [UInt(1)] public bool Enabled { get; set; }
}
