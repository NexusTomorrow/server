using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[Message(GameMessageOpcode.ServerGenericMapShow)]
[WireMessage(GameMessageOpcode.ServerGenericMapShow, Direction.ServerToClient)]
public sealed partial class ServerGenericMapShow : IWritable
{
    [UInt(14)] public uint GenericMapId { get; set; }
    [Array(CountBits = 32)] public List<GenericMapNode> Nodes { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GenericMapId, 14u);
        writer.Write(Nodes.Count);
        Nodes.ForEach(node => node.Write(writer));
    }
}
