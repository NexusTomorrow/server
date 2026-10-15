using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ServerCostumeItemUnlockMultiple)]
[WireMessage(GameMessageOpcode.ServerCostumeItemUnlockMultiple, Direction.ServerToClient)]
public sealed partial class ServerCostumeItemUnlockMultiple : IWritable
{
    [UInt(32)] public CostumeUnlockResult Result { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Item2Ids { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 32u);
        writer.Write(Item2Ids.Count);
        Item2Ids.ForEach(i => writer.Write(i));
    }
}
