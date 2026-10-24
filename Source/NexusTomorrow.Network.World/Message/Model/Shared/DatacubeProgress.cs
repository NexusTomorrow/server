using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// One datacube's progress: the parts of it the player has unlocked.
/// </summary>
[WireStruct]
public sealed partial class DatacubeProgress
{
    /// <summary>
    /// The datacube, as a Datacube id.
    /// </summary>
    [UInt(14)] public ushort DatacubeId { get; set; }

    /// <summary>
    /// One bit per part unlocked; a datacube of the Chronicle type is complete when the set bits reach its unlockCount.
    /// </summary>
    [UInt(32)] public uint UnlockedMask { get; set; }
}
