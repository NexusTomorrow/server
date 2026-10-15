using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerItemDelete)]
[WireMessage(GameMessageOpcode.ServerItemDelete, Direction.ServerToClient)]
public sealed partial class ServerItemDelete : IWritable
{
    [UInt(64)] public ulong Guid { get; set; }
    [UInt(6)] public ItemUpdateReason Reason { get; set; } = ItemUpdateReason.Loot;

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(Reason, 6u);
    }
}
