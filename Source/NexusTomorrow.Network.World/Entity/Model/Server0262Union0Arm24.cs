using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 36.
/// </summary>
[WireStruct]
[UnionCase(36)]
public sealed partial class Server0262Union0Arm24 : IServer0262Union0
{
    /// <summary>
    /// One unit in the list.
    /// </summary>
    [WireStruct]
    public sealed partial class Server0262Part3
    {
        /// <summary>
        /// A unit's id.
        /// </summary>
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [UInt(18)] public uint Unknown0 { get; set; }

    /// <summary>
    /// Unit ids. The client marks the unit when the player's unit is among them.
    /// </summary>
    [Array(CountBits = 16)] public List<Server0262Part3> Unknown1 { get; } = [];
}
