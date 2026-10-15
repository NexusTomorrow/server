using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ServerCostumeItemUnlock)]
public class ServerCostumeItemUnlock : IWritable
{
    public uint Item2Id { get; set; }
    public CostumeUnlockResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Item2Id, 18u);
        writer.Write(Result, 32u);
    }
}
