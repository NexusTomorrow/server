using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingNeighbors)]
public class ServerHousingNeighbours : IWritable
{
    public List<Neighbour> Neighbours { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Neighbours.Count);
        Neighbours.ForEach(n => n.Write(writer));
    }
}
