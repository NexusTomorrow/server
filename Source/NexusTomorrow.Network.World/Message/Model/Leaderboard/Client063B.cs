using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Client063B, Direction.ClientToServer)]
public sealed partial class Client063B
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
