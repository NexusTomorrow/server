using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// One datacube volume's progress: the entries of it the player has unlocked.
/// </summary>
[WireStruct]
public sealed partial class DatacubeVolumeProgress
{
    /// <summary>
    /// The volume, as a DatacubeVolume id.
    /// </summary>
    [UInt(14)] public ushort VolumeId { get; set; }

    /// <summary>
    /// One bit per entry unlocked; the volume is complete when the set bits reach its entry count.
    /// </summary>
    [UInt(32)] public uint UnlockedMask { get; set; }
}
