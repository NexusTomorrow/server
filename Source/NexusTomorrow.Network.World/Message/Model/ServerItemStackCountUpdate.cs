using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerItemStackCountUpdate)]
[WireMessage(GameMessageOpcode.ServerItemStackCountUpdate, Direction.ServerToClient)]
public sealed partial class ServerItemStackCountUpdate : IWritable
{
    [UInt(64)] public ulong Guid { get; set; }
    [UInt(32)] public uint StackCount { get; set; }
    [UInt(6)] public ItemUpdateReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(StackCount);
        writer.Write(Reason, 6u);
    }
}
