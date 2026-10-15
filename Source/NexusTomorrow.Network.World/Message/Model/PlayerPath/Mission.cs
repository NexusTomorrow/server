using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireStruct]
public sealed partial class Mission
{
    [UInt(15)] public uint PathMissionId { get; set; }
    [UInt(1)] public bool Completed { get; set; }
    [UInt(32)] public uint ObjectiveCompletionFlags { get; set; }
    [UInt(32)] public uint StateFlags { get; set; }
    [UInt(3)] public PathMissionState State { get; set; }
    [UInt(32)] public uint GiverUnitId { get; set; }
}
