using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientQuestShare, Direction.ClientToServer)]
public sealed partial class ClientQuestShare
{
    [UInt(15)] public ushort QuestId { get; private set; }
}
