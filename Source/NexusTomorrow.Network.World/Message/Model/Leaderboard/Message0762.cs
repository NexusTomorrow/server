using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Message0762, Direction.Both)]
public sealed partial class Message0762
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 8), WideString] public List<string> Unknown1 { get; } = [];
}
