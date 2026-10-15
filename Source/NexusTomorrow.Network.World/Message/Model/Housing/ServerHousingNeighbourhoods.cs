using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingNeighborhoods)]
[WireMessage(GameMessageOpcode.ServerHousingNeighborhoods, Direction.ServerToClient)]
public sealed partial class ServerHousingNeighbourhoods : IWritable
{
    [UInt(14)] public ushort Unused { get; set; }
    [Array(CountBits = 32)] public List<Neighbourhood> Neighbourhoods { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unused, 14u);
        writer.Write(Neighbourhoods.Count);
        foreach (Neighbourhood Neighbourhood in Neighbourhoods)
            Neighbourhood.Write(writer);
    }
}
