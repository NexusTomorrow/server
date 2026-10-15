using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[Message(GameMessageOpcode.ServerHazardList)]
[WireMessage(GameMessageOpcode.ServerHazardList, Direction.ServerToClient)]
public sealed partial class ServerHazardList : IWritable
{
    [Array(CountBits = 32)] public List<Hazard> Hazards { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Hazards.Count);
        Hazards.ForEach(hazard => hazard.Write(writer));
    }
}
