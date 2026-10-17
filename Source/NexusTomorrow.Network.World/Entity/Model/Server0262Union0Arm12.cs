using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 18.
/// </summary>
[WireStruct]
[UnionCase(18)]
public sealed partial class Server0262Union0Arm12 : IServer0262Union0
{
    /// <summary>
    /// The unit's creature, as an id in the client's Creature2 table, or 0 for none. The client refuses the unit when it has no creature
    /// for a non-zero id.
    /// </summary>
    [UInt(18)] public uint Unknown0 { get; set; }

    /// <summary>
    /// An id the client looks up, in the way <see cref="Unknown3"/> selects, for the unit's name. When that finds nothing, the unit takes
    /// its creature's name.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// Selects how the client looks up <see cref="Unknown1"/> for the unit's name. The client uses 0, 2, 6, 7, 8 and 9; with any other
    /// value the unit takes its creature's name.
    /// </summary>
    [UInt(4)] public byte Unknown3 { get; set; }
}
