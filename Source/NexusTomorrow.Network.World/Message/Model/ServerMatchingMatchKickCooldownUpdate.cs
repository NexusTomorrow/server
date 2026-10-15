using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// If MatchQueueResult is GlobalKickCooldown, no wait time is needs to be sent as the message does not use the field
// If MatchQueueResult is that PersonalKickCooldown, uses the WaitTime to update client views
[WireMessage(GameMessageOpcode.ServerMatchingMatchKickCooldownUpdate, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchKickCooldownUpdate
{
    [UInt(6)] public MatchingQueueResult Result { get; set; }
    [UInt(32)] public uint WaitTimeBeforeVoteMS { get; set; }
}
