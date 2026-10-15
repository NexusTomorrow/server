using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientQuestSetTracked, Direction.ClientToServer)]
public sealed partial class ClientQuestSetTracked
{
    [UInt(15)] public ushort QuestId { get; private set; }
    [UInt(1)] public bool Tracked { get; private set; }
}
