using System.Diagnostics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientStatisticsConnection, Direction.ClientToServer)]
public sealed partial class ClientStatisticsConnection
{
    [UInt(32)] public uint AverageRoundTripInMs { get; private set; }
    [UInt(32)] public uint BytesReceivedPerSecond { get; private set; }
    [UInt(32)] public uint BytesSentPerSecond { get; private set; }
    [UInt(1)] public bool Unknown { get; private set; }
    [UInt(31)] public uint UnitHashTableEntryCount { get; private set; }
}
