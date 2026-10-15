using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientQuestRetry, Direction.ClientToServer)]
public sealed partial class ClientQuestRetry
{
    [UInt(15)] public ushort QuestId { get; private set; }
}
