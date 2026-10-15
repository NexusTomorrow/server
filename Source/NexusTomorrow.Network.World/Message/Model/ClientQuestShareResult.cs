using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientQuestShareResult, Direction.ClientToServer)]
public sealed partial class ClientQuestShareResult
{
    [UInt(15)] public ushort QuestId { get; private set; }
    [UInt(1)] public bool Result { get; private set; }
}
