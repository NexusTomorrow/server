using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestShared)]
[WireMessage(GameMessageOpcode.ServerQuestShared, Direction.ServerToClient)]
public sealed partial class ServerQuestShared : IWritable
{
    [UInt(32)] public uint SharerUnitId { get; set; }
    [UInt(15)] public uint QuestId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SharerUnitId);
        writer.Write(QuestId, 15u);
    }
}
