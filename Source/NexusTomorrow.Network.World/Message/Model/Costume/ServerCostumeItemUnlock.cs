using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ServerCostumeItemUnlock)]
[WireMessage(GameMessageOpcode.ServerCostumeItemUnlock, Direction.ServerToClient)]
public sealed partial class ServerCostumeItemUnlock : IWritable
{
    [UInt(18)] public uint Item2Id { get; set; }
    [UInt(32)] public CostumeUnlockResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Item2Id, 18u);
        writer.Write(Result, 32u);
    }
}
