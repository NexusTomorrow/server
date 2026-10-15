using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingCrateAllDecorOnPlot, Direction.ServerToClient)]
public sealed partial class ServerHousingCrateAllDecorOnPlot
{
    public Identity ResidenceIdentity { get; set; }
    [UInt(32)] public uint PlotIndex { get; set; }
}
