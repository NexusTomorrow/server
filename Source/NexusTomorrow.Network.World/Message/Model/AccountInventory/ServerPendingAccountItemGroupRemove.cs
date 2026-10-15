using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerPendingAccountItemGroupRemove)]
[WireMessage(GameMessageOpcode.ServerPendingAccountItemGroupRemove, Direction.ServerToClient)]
public sealed partial class ServerPendingAccountItemGroupRemove : IWritable
{
    [UInt(64)] public ulong PendingItemGroupId { get; set; } // a bit of a guess
    [UInt(1)] public bool Unused { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PendingItemGroupId);
        writer.Write(Unused);
    }
}
