using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingQueueLeave)]
[WireMessage(GameMessageOpcode.ClientMatchingQueueLeave, Direction.ClientToServer)]
public sealed partial class ClientMatchingQueueLeave : IReadable
{
    [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; private set; }

    public void Read(GamePacketReader reader)
    {
        MatchType = reader.ReadEnum<Game.Static.Matching.MatchType>(5u);
    }
}
