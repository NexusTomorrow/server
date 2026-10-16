using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Client0701, Direction.ClientToServer)]
public sealed partial class Client0701
{
    [UInt(2)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
