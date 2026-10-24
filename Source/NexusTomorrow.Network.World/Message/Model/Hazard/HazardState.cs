using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

/// <summary>
/// The live state of one hazard: its meter and the unit its minimap marker follows. The client keeps one for each hazard id.
/// </summary>
[WireStruct]
public sealed partial class HazardState
{
    /// <summary>
    /// The hazard, by id in the Hazard table. It keys the record, and HazardsLib takes the hazard's row by it.
    /// </summary>
    [VerifiedMember, UInt(14)] public uint HazardId { get; set; }

    [UInt(3)] public byte Unknown1 { get; set; }

    /// <summary>
    /// The hazard's meter. Hazards.lua draws it as the bar's progress, and the client's meter and threshold checks read it.
    /// </summary>
    [VerifiedMember, Float32] public float MeterValue { get; set; }

    /// <summary>
    /// The meter's base maximum. The client scales it with the hazard's <see cref="HazardMaxModifier"/> entries into the maximum
    /// HazardsLib reports, and the threshold check takes the meter as a percent of it.
    /// </summary>
    [VerifiedMember, Float32] public float MeterMax { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint Unknown5 { get; set; }

    /// <summary>
    /// The unit the hazard's minimap marker follows. The client hands it to the add-ons as unitMarker when the marker is shown and as
    /// unitRemoved when it is removed, and MiniMap.lua and ZoneMap.lua add and remove it as a map icon.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint MarkerUnitId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown7 { get; set; }

    [UInt(1)] public bool Unknown8 { get; set; }
    [UInt(1)] public bool Unknown9 { get; set; }
    [UInt(1)] public bool Unknown10 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown11 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown12 { get; set; }
}
