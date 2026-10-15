using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountItemRemove)]
[WireMessage(GameMessageOpcode.ServerAccountItemRemove, Direction.ServerToClient)]
public sealed partial class ServerAccountItemRemove : IWritable
{
    [UInt(64)] public ulong UserInventoryId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UserInventoryId);
    }
}
