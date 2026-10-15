using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ServerCostumeSaveResult)]
public class ServerCostumeSave : IWritable
{
    public int Index { get; set; }
    public CostumeType Type { get; set; }
    public CostumeSaveResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Index);
        writer.Write(Type, 2u);
        writer.Write(Result, 4u);
    }
}
