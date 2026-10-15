using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Updates the wait time for a specific match type
[Message(GameMessageOpcode.ServerMatchingAverageWaitTimeUpdate)]
[WireMessage(GameMessageOpcode.ServerMatchingAverageWaitTimeUpdate, Direction.ServerToClient)]
public sealed partial class ServerMatchingAverageWaitTimeUpdate : IWritable
{
    [UInt(5)] public Game.Static.Matching.MatchType Type { get; set; }
    [UInt(32)] public uint AverageWaitTime { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 5u);
        writer.Write(AverageWaitTime);
    }
}
