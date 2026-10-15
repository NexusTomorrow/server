using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityPropertiesUpdate)]
public class ServerEntityPropertiesUpdate : IWritable
{
    public uint UnitId { get; set; }
    public List<PropertyValue> Properties { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Properties.Count, 8u);
        Properties.ForEach(u => u.Write(writer));
    }
}
