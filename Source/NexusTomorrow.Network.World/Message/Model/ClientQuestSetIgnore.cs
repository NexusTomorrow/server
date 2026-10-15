using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientQuestSetIgnore)]
[WireMessage(GameMessageOpcode.ClientQuestSetIgnore, Direction.ClientToServer)]
public sealed partial class ClientQuestSetIgnore : IReadable
{
    [UInt(15)] public ushort QuestId { get; private set; }
    [UInt(1)] public bool Ignore { get; private set; }

    public void Read(GamePacketReader reader)
    {
        QuestId = reader.ReadUShort(15u);
        Ignore = reader.ReadBit();
    }
}
