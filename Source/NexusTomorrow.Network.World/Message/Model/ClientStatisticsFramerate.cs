using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientStatisticsFramerate, Direction.ClientToServer)]
public sealed partial class ClientStatisticsFramerate
{
    [UInt(1)] public bool Unknown1 { get; private set; }
    [UInt(31)] public uint RecentAverageFrameTime { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
    [UInt(31)] public uint HighestTimeFrame { get; private set; }
    public Position PositionAtSlowestFrame { get; private set; } = new Position();
    [Float32] public float GameTimeStepPreecedingSlowestFrame { get; private set; } // value is often a negative value, unsure why
    [UInt(1)] public bool Unknown3 { get; private set; }
    [UInt(31)] public uint EntireSessionAverageFrameTime { get; private set; }
}
