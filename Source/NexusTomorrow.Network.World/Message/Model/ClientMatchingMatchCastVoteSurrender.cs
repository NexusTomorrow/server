using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Only sent if a surrender vote is active
// If MatchType is a PvP type, is a vote to surrender
// If MatchType is a PvE type, is a vote to disband
[Message(GameMessageOpcode.ClientMatchingMatchCastVoteSurrender)]
[WireMessage(GameMessageOpcode.ClientMatchingMatchCastVoteSurrender, Direction.ClientToServer)]
public sealed partial class ClientMatchingMatchCastVoteSurrender : IReadable
{
    [UInt(1)] public bool Vote { get; private set; } // true = yes, false = no

    public void Read(GamePacketReader reader)
    {
        Vote = reader.ReadBit();
    }
}
