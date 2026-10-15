using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountOperationResult)]
[WireMessage(GameMessageOpcode.ServerAccountOperationResult, Direction.ServerToClient)]
public sealed partial class ServerAccountOperationResult : IWritable
{
    [UInt(32)] public AccountOperation Operation { get; set; }
    [Pad(32)] public Pad OperationHigh { get; }
    [UInt(32)] public AccountOperationResult Result { get; set; }
    [Pad(32)] public Pad ResultHigh { get; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Operation);
        writer.Write(Result);
    }
}
