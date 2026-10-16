using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Server05CF, Direction.ServerToClient)]
public sealed partial class Server05CF
{
    [UInt(4)] public byte Unknown0 { get; set; }
}
