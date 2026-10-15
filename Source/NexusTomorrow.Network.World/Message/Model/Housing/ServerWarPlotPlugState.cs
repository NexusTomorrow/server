using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerWarPlotPlugState, Direction.ServerToClient)]
public sealed partial class ServerWarPlotPlugState
{
    [UInt(32)] public uint CratedDecorCount { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(16)] public ushort PlacedDecorCount { get; set; }
    [Array(CountBits = 8)] public List<WarPlotPlug> Plugs { get; set; } = [];
}
