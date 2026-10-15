using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountOperationResult)]
public class ServerAccountOperationResult : IWritable
{
    public AccountOperation Operation { get; set; }
    public AccountOperationResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Operation);
        writer.Write(Result);
    }
}
