using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingQueueRandomParty)]
[WireMessage(GameMessageOpcode.ClientMatchingQueueRandomParty, Direction.ClientToServer)]
public sealed partial class ClientMatchingQueueRandomParty
{
    [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; private set; }
    [UInt(32)] public MatchingQueueFlags Flags { get; private set; }
    [UInt(32)] public Role Roles { get; private set; }

    public void Read(GamePacketReader reader)
    {
        MatchType = reader.ReadEnum<Game.Static.Matching.MatchType>(5u);
        Flags = reader.ReadEnum<MatchingQueueFlags>(32u);
        Roles = reader.ReadEnum<Role>(32u);
    }
}
