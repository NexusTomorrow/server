using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

// One count, then every Item2Id, then every Count: element i of each list is one harvested item.
[WireMessage(GameMessageOpcode.ServerHousingHarvestItemsSentToOwner, Direction.ServerToClient)]
public sealed partial class ServerHousingHarvestItemsSentToOwner
{
    [UInt(32), Count] public uint HarvestedItemCount { get; set; }
    [Array(CountFrom = nameof(HarvestedItemCount)), UInt(32)] public List<uint> Item2Ids { get; set; } = [];
    [Array(CountFrom = nameof(HarvestedItemCount)), UInt(32)] public List<uint> Counts { get; set; } = [];
}
