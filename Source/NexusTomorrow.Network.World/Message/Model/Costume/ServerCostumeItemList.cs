using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ServerCostumeItemList)]
[WireMessage(GameMessageOpcode.ServerCostumeItemList, Direction.ServerToClient)]
public sealed partial class ServerCostumeItemList : IWritable
{
    [Array(CountBits = 32), UInt(32)] public List<uint> Item2Ids { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Item2Ids.Count);
        Item2Ids.ForEach(i => writer.Write(i));
    }
}
