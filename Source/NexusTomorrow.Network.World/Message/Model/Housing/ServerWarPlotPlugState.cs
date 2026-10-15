using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerWarPlotPlugState)]
[WireMessage(GameMessageOpcode.ServerWarPlotPlugState, Direction.ServerToClient)]
public sealed partial class ServerWarPlotPlugState : IWritable
{
    [UInt(32)] public uint CratedDecorCount { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(16)] public ushort PlacedDecorCount { get; set; }
    [Array(CountBits = 8)] public List<WarPlotPlug> Plugs { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CratedDecorCount);
        writer.Write(Unknown1);
        writer.Write(PlacedDecorCount);
        writer.Write(Plugs.Count, 8u);
        Plugs.ForEach(plug => plug.Write(writer));
    }
}
