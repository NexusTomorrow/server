using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Message0634, Direction.Both)]
public sealed partial class Message0634
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
