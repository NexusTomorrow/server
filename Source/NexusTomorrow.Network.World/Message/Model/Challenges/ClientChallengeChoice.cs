using NexusTomorrow.Game.Static.Challenges;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[WireMessage(GameMessageOpcode.ClientChallengeChoice, Direction.ClientToServer)]
public sealed partial class ClientChallengeChoice
{
    [UInt(14)] public ushort ChallengeId { get; private set; }
    [UInt(5)] public ChallengeChoice Choice { get; private set; }
    [UInt(32)] public uint Unused { get; private set; }
}
