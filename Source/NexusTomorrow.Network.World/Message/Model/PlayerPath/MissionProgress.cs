using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// The first four fields of a <see cref="Mission"/>, which the mission update and episode progress messages carry without its state and giver.
/// </summary>
[WireStruct]
public sealed partial class MissionProgress
{
    [UInt(15)] public uint PathMissionId { get; set; }
    [UInt(1)] public bool Completed { get; set; }
    [UInt(32)] public uint ObjectiveCompletionFlags { get; set; }
    [UInt(32)] public uint StateFlags { get; set; }
}
