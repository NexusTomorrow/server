using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[Message(GameMessageOpcode.ServerInspectPlayerResponse)]
public class ServerInspectPlayerResponse : IWritable
{
    public uint UnitId { get; set; }
    public List<Item> Items { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);

        writer.Write(Items.Count, 5u);
        Items.ForEach(item => item.Write(writer));
    }
}
