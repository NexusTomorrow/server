using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ServerCostumeSaveResult)]
[WireMessage(GameMessageOpcode.ServerCostumeSaveResult, Direction.ServerToClient)]
public sealed partial class ServerCostumeSave : IWritable
{
    [UInt(32)] public int Index { get; set; }
    [UInt(2)] public CostumeType Type { get; set; }
    [UInt(4)] public CostumeSaveResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Index);
        writer.Write(Type, 2u);
        writer.Write(Result, 4u);
    }
}
