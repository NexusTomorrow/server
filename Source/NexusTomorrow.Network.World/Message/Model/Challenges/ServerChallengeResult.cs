using NexusTomorrow.Game.Static.Challenges;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[WireMessage(GameMessageOpcode.ServerChallengeResult, Direction.ServerToClient)]
public sealed partial class ServerChallengeResult
{
    [UInt(14)] public ushort ChallengeId { get; set; }
    [UInt(5)] public ChallengeResult Result { get; set; }
    [UInt(32)] public int Data { get; set; } // Sometimes localizedStringId, sometimes tier value
}
