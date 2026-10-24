using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Enables or disables one node of a generic map.
/// </summary>
[WireMessage(GameMessageOpcode.Server0737, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GenericMapNodeChanged
{
    /// <summary>
    /// The node and its new state.
    /// </summary>
    public GenericMapNodeState Node { get; set; } = new();
}
