using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientQuestAccept, Direction.ClientToServer)]
public sealed partial class ClientQuestAccept
{
    public ItemLocation ItemLocation { get; } = new();
    [UInt(15)] public ushort QuestId { get; private set; }
}
