using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[WireMessage(GameMessageOpcode.ServerChallengeShared, Direction.ServerToClient)]
public sealed partial class ServerChallengeShared
{
    [UInt(14)] public ushort ChallengeId { get; set; }
    [UInt(32)] public uint SharerUnitId { get; set; }
}
