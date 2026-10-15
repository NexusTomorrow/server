using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

// Only sent if a kick vote is active
[WireMessage(GameMessageOpcode.ClientMatchingMatchCastVoteKick, Direction.ClientToServer)]
public sealed partial class ClientMatchingMatchCastVoteKick
{
    public Identity MemberToKick { get; private set; } = new(); // The match member to kick
    [UInt(1)] public bool Vote { get; private set; } // true = yes, false = no
}
