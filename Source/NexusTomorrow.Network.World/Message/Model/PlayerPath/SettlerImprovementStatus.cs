using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// One settler improvement's status in a hub, under the improvement's id.
/// </summary>
[WireStruct]
public sealed partial class SettlerImprovementStatus
{
    /// <summary>
    /// The improvement, by the id the hub files it under.
    /// </summary>
    [UInt(14)] public ushort ImprovementId { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }
}
