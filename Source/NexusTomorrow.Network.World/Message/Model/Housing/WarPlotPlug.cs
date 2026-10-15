using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public sealed partial class WarPlotPlug
{
    [UInt(8)] public byte Index { get; set; }
    [UInt(32)] public uint Health { get; set; }
    [UInt(32)] public uint HealthMax { get; set; }
    [UInt(2)] public byte Tier { get; set; }
}
