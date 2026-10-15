using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerItemDelete)]
public class ServerItemDelete : IWritable
{
    public ulong Guid { get; set; }
    public ItemUpdateReason Reason { get; set; } = ItemUpdateReason.Loot;

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(Reason, 6u);
    }
}
