using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientQuestComplete)]
[WireMessage(GameMessageOpcode.ClientQuestComplete, Direction.ClientToServer)]
public sealed partial class ClientQuestComplete : IReadable
{
    [UInt(15)] public ushort QuestId { get; private set; }
    [UInt(15)] public ushort RewardSelection { get; private set; }
    [UInt(1)] public bool IsCommunicatorMsg { get; private set; }

    public void Read(GamePacketReader reader)
    {
        QuestId = reader.ReadUShort(15u);
        RewardSelection = reader.ReadUShort(15u);
        IsCommunicatorMsg = reader.ReadBit();
    }
}
