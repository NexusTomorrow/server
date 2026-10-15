using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingNeighbourhoods)]
public class ServerHousingNeighbourhoods : IWritable
{
    public ushort Unused { get; set; }
    public List<Neighbourhood> Neighbourhoods { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unused, 14u);
        writer.Write(Neighbourhoods.Count);
        foreach (Neighbourhood Neighbourhood in Neighbourhoods)
            Neighbourhood.Write(writer);
    }
}
