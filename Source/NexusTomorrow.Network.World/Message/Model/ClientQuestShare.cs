using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientQuestShare)]
[WireMessage(GameMessageOpcode.ClientQuestShare, Direction.ClientToServer)]
public sealed partial class ClientQuestShare : IReadable
{
    [UInt(15)] public ushort QuestId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        QuestId = reader.ReadUShort(15u);
    }
}
