using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Tells the server which node of a generic map the player chose.
/// </summary>
[WireMessage(GameMessageOpcode.Client0754, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GenericMapNodeChoose
{
    /// <summary>
    /// The chosen node, as a GenericMapNode id.
    /// </summary>
    [UInt(14)] public ushort GenericMapNodeId { get; private set; }
}
