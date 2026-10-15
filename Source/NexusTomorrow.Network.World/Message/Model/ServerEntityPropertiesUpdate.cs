using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityPropertiesUpdate)]
[WireMessage(GameMessageOpcode.ServerEntityPropertiesUpdate, Direction.ServerToClient)]
public sealed partial class ServerEntityPropertiesUpdate : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [Array(CountBits = 8)] public List<PropertyValue> Properties { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Properties.Count, 8u);
        Properties.ForEach(u => u.Write(writer));
    }
}
