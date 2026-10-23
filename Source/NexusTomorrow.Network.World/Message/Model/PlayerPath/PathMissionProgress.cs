using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// One mission's progress, under its PathMission id.
/// </summary>
[WireStruct]
public sealed partial class PathMissionProgress
{
    /// <summary>
    /// The mission, a PathMission table id.
    /// </summary>
    [UInt(15)] public uint MissionId { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }
}
