using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingPenaltyUpdated)]
[WireMessage(GameMessageOpcode.ServerMatchingPenaltyUpdated, Direction.ServerToClient)]
public sealed partial class ServerMatchingPenaltyUpdated : IWritable
{
    [FixedArray(16), UInt(32)] public uint[] MatchingPenaltyTimesMS { get; set; } = new uint[16]; // These are the penalty times for each MatchType for the player
                                                                                                  // Array is ordered by Game.Static.Matching.MatchType
    public void Write(GamePacketWriter writer)
    {
        foreach (uint penaltyTime in MatchingPenaltyTimesMS)
        {
            writer.Write(penaltyTime);
        }
    }
}
