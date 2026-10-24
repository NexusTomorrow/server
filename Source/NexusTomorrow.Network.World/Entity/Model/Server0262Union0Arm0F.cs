using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 15.
/// </summary>
[WireStruct]
[UnionCase(15)]
public sealed partial class Server0262Union0Arm0F : IServer0262Union0
{
    /// <summary>
    /// The socket the unit's world plugs into, as an id in the client's WorldSocket table. The client places the plug in the socket as it
    /// creates the unit, refuses the unit when it cannot, and removes the plug when the unit is destroyed.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort WorldSocketId { get; set; }

    /// <summary>
    /// The world the client plugs into the socket, as an id in the client's World table. The client refuses the unit when it cannot look
    /// the world up.
    /// </summary>
    [VerifiedMember, UInt(15)] public ushort WorldId { get; set; }

    [UInt(6)] public byte Unknown2 { get; set; }
}
