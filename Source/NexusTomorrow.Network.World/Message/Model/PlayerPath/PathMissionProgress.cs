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

    /// <summary>
    /// Whether the mission is complete, which PathMission:IsComplete reads; when it rises the client raises the mission's completion
    /// events.
    /// </summary>
    [UInt(1)] public bool IsComplete { get; set; }

    /// <summary>
    /// The mission's progress, which PathMission:GetNumCompleted returns: as it is for some mission types, and for most one bit per
    /// completed part.
    /// </summary>
    [UInt(32)] public uint Progress { get; set; }

    /// <summary>
    /// State bits whose meaning depends on the mission's type, behind PathMission:GetMissionState and PathMission:IsStarted.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }
}
