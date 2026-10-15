using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerItemStackCountUpdate)]
public class ServerItemStackCountUpdate : IWritable
{
    public ulong Guid { get; set; }
    public uint StackCount { get; set; }
    public ItemUpdateReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(StackCount);
        writer.Write(Reason, 6u);
    }
}
