using System.Diagnostics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientStatisticsConnection)]
[WireMessage(GameMessageOpcode.ClientStatisticsConnection, Direction.ClientToServer)]
public sealed partial class ClientStatisticsConnection : IReadable
{
    [UInt(32)] public uint AverageRoundTripInMs { get; private set; }
    [UInt(32)] public uint BytesReceivedPerSecond { get; private set; }
    [UInt(32)] public uint BytesSentPerSecond { get; private set; }
    [UInt(1)] public bool Unknown { get; private set; }
    [UInt(31)] public uint UnitHashTableEntryCount { get; private set; }

    public void Read(GamePacketReader reader)
    {
        AverageRoundTripInMs = reader.ReadUInt();
        BytesReceivedPerSecond = reader.ReadUInt();
        BytesSentPerSecond = reader.ReadUInt();

        uint temp = reader.ReadUInt();
        Unknown = (temp & 0x00000001) != 0;
        UnitHashTableEntryCount = temp >> 1;
    }
}
