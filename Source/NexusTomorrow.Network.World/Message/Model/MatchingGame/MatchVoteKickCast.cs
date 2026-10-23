using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Client0617, Direction.ClientToServer)]
public sealed partial class MatchVoteKickCast
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    [UInt(1)] public bool Unknown1 { get; private set; }
}
