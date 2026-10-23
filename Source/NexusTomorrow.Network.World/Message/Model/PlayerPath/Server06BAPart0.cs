using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireStruct]
public sealed partial class Server06BAPart0
{
    [UInt(15)] public uint Unknown0 { get; set; }

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

    [UInt(3)] public byte Unknown4 { get; set; }

    [UInt(32)] public uint Unknown5 { get; set; }
}
