using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

// One count, then every Item2Id, then every Count: element i of each list is one harvested item.
[Message(GameMessageOpcode.ServerHousingHarvestItemsSentToOwner)]
[WireMessage(GameMessageOpcode.ServerHousingHarvestItemsSentToOwner, Direction.ServerToClient)]
public sealed partial class ServerHousingHarvestItemsSentToOwner : IWritable
{
    [UInt(32), Count] public uint HarvestedItemCount { get; set; }
    [Array(CountFrom = nameof(HarvestedItemCount)), UInt(32)] public List<uint> Item2Ids { get; set; } = [];
    [Array(CountFrom = nameof(HarvestedItemCount)), UInt(32)] public List<uint> Counts { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Item2Ids.Count);
        Item2Ids.ForEach(item2Id => writer.Write(item2Id));
        Counts.ForEach(count => writer.Write(count));
    }
}
