using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingCrateAllDecorOnPlot)]
public class ServerHousingCrateAllDecorOnPlot : IWritable
{
    public Identity ResidenceIdentity { get; set; }
    public uint PlotIndex { get; set; }

    public void Write(GamePacketWriter writer)
    {
        ResidenceIdentity.Write(writer);
        writer.Write(PlotIndex);
    }
}
