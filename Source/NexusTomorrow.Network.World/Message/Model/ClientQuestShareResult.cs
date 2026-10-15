using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientQuestShareResult)]
[WireMessage(GameMessageOpcode.ClientQuestShareResult, Direction.ClientToServer)]
public sealed partial class ClientQuestShareResult : IReadable
{
    [UInt(15)] public ushort QuestId { get; private set; }
    [UInt(1)] public bool Result { get; private set; }

    public void Read(GamePacketReader reader)
    {
        QuestId = reader.ReadUShort(15u);
        Result = reader.ReadBit();
    }
}
