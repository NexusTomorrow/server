using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingCrateAllDecorOnPlot)]
[WireMessage(GameMessageOpcode.ServerHousingCrateAllDecorOnPlot, Direction.ServerToClient)]
public sealed partial class ServerHousingCrateAllDecorOnPlot : IWritable
{
    public Identity ResidenceIdentity { get; set; }
    [UInt(32)] public uint PlotIndex { get; set; }

    public void Write(GamePacketWriter writer)
    {
        ResidenceIdentity.Write(writer);
        writer.Write(PlotIndex);
    }
}
