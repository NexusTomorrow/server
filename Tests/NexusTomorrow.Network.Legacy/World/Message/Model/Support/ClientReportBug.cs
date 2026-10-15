using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[Message(GameMessageOpcode.ClientReportBug)]
public class ClientReportBug : IReadable
{
    public BugCategory BugCategoryId { get; private set; }
    public uint SelectedUnitId { get; private set; }
    public uint Quest2Id { get; private set; }
    public string Description { get; private set; }

    public void Read(GamePacketReader reader)
    {
        BugCategoryId = reader.ReadEnum<BugCategory>(16u);
        SelectedUnitId = reader.ReadUInt();
        Quest2Id = reader.ReadUInt();
        Description = reader.ReadWideString();
    }
}
