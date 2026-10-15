using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[Message(GameMessageOpcode.ClientReportBug)]
[WireMessage(GameMessageOpcode.ClientReportBug, Direction.ClientToServer)]
public sealed partial class ClientReportBug : IReadable
{
    [UInt(16)] public BugCategory BugCategoryId { get; private set; }
    [UInt(32)] public uint SelectedUnitId { get; private set; }
    [UInt(32)] public uint Quest2Id { get; private set; }
    [WideString] public string Description { get; private set; }

    public void Read(GamePacketReader reader)
    {
        BugCategoryId = reader.ReadEnum<BugCategory>(16u);
        SelectedUnitId = reader.ReadUInt();
        Quest2Id = reader.ReadUInt();
        Description = reader.ReadWideString();
    }
}
