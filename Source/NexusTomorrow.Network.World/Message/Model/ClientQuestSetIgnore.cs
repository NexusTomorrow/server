using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientQuestSetIgnore, Direction.ClientToServer)]
public sealed partial class ClientQuestSetIgnore
{
    [UInt(15)] public ushort QuestId { get; private set; }
    [UInt(1)] public bool Ignore { get; private set; }
}
