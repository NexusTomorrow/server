using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingNeighbors)]
[WireMessage(GameMessageOpcode.ServerHousingNeighbors, Direction.ServerToClient)]
public sealed partial class ServerHousingNeighbours : IWritable
{
    [Array(CountBits = 32)] public List<Neighbour> Neighbours { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Neighbours.Count);
        Neighbours.ForEach(n => n.Write(writer));
    }
}
