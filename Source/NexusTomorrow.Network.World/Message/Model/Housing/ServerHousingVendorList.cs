using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingVendorList, Direction.ServerToClient)]
public sealed partial class ServerHousingVendorList
{
    [WireStruct]
    public sealed partial class PlugItem
    {
        [UInt(64)] public ulong SourceId { get; set; }
        [UInt(32)] public uint PlugItemId { get; set; }
        [UInt(32)] public uint Cost { get; set; }
        [UInt(32)] public uint PlugItemFlags { get; set; }
    }

    [Array(CountBits = 32)] public List<PlugItem> PlugItems { get; set; } = [];
    [UInt(2)] public byte ListType { get; set; } = 0; // must be zero or else the client ignores the contents of the message
}
