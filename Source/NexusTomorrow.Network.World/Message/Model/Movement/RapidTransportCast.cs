using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Casts Rapid Transport to a taxi node, from the taxi map.
/// </summary>
/// <remarks>
/// The client looks the node up in its TaxiNode table before it sends.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0141, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class RapidTransportCast
{
    /// <summary>
    /// The node to travel to, by its TaxiNode table id.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort TaxiNodeId { get; private set; }

    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint ClientCastId { get; private set; }
}
