using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Server057A, Direction.ServerToClient)]
public sealed partial class Server057A
{
    [UInt(4)] public byte Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [Array(CountBits = 32)] public List<Server057APart0> Unknown2 { get; set; } = [];
}
