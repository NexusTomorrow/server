using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountItemInProgressRemove)]
[WireMessage(GameMessageOpcode.ServerAccountItemInProgressRemove, Direction.ServerToClient)]
public sealed partial class ServerAccountItemInProgressRemove : IWritable
{
    [UInt(32)] public uint AccountId { get; set; } // is the AccountId sent ServerRealmInfo (0x3DB), filled by server but not used by client
    [UInt(64)] public ulong UserInventoryId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountId);
        writer.Write(UserInventoryId);
    }
}
