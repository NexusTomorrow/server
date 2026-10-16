using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Client0799, Direction.ClientToServer)]
public sealed partial class Client0799
{
    [UInt(4)] public byte Unknown0 { get; private set; }
}
