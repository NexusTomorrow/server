using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientQuestAbandon, Direction.ClientToServer)]
public sealed partial class ClientQuestAbandon
{
    [UInt(15)] public ushort QuestId { get; private set; }
}
