using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ServerCostumeItemUnlockMultiple)]
public class ServerCostumeItemUnlockMultiple : IWritable
{
    public CostumeUnlockResult Result { get; set; }
    public List<uint> Item2Ids { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 32u);
        writer.Write(Item2Ids.Count);
        Item2Ids.ForEach(i => writer.Write(i));
    }
}
