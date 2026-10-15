using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public partial class Neighbourhood
{
    [UInt(64)] public ulong NeighbourhoodId { get; set; }
    [UInt(14)] public ushort HousingNeighbourhoodInfoId { get; set; }
    [UInt(14)] public ushort ParentRealmId { get; set; }
    [UInt(64)] public ulong GuildIdOwner { get; set; }
    [WideString] public string Name { get; set; }
    [UInt(32)] public uint Population { get; set; }
    [UInt(32)] public uint MaxPopulation { get; set; }
    [UInt(32)] public uint Cost { get; set; }
}
