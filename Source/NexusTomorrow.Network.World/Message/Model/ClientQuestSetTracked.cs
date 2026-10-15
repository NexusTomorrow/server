using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientQuestSetTracked)]
[WireMessage(GameMessageOpcode.ClientQuestSetTracked, Direction.ClientToServer)]
public sealed partial class ClientQuestSetTracked : IReadable
{
    [UInt(15)] public ushort QuestId { get; private set; }
    [UInt(1)] public bool Tracked { get; private set; }

    public void Read(GamePacketReader reader)
    {
        QuestId = reader.ReadUShort(15u);
        Tracked = reader.ReadBit();
    }
}
