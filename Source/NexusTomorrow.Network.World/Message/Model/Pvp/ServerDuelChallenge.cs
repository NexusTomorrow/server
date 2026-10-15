using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// Send to all observers in the nearby area. Client handles the correctness of the UI messages.
[WireMessage(GameMessageOpcode.ServerDuelChallenge, Direction.ServerToClient)]
public sealed partial class ServerDuelChallenge
{
    [UInt(32)] public uint ChallengerUnitId { get; set; }
    [UInt(32)] public uint OpponentUnitId { get; set; }
}
