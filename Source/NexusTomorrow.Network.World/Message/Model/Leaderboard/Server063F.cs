using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Server063F, Direction.ServerToClient)]
public sealed partial class Server063F
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
