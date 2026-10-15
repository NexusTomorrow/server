using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingPenaltyUpdated, Direction.ServerToClient)]
public sealed partial class ServerMatchingPenaltyUpdated
{
    [FixedArray(16), UInt(32)] public uint[] MatchingPenaltyTimesMS { get; set; } = new uint[16]; // These are the penalty times for each MatchType for the player
}
