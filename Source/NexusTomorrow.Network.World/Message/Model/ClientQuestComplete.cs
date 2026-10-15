using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientQuestComplete, Direction.ClientToServer)]
public sealed partial class ClientQuestComplete
{
    [UInt(15)] public ushort QuestId { get; private set; }
    [UInt(15)] public ushort RewardSelection { get; private set; }
    [UInt(1)] public bool IsCommunicatorMsg { get; private set; }
}
