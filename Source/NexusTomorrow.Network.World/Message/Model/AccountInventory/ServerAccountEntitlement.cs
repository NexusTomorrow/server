using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountEntitlement)]
[WireMessage(GameMessageOpcode.ServerAccountEntitlement, Direction.ServerToClient)]
public sealed partial class ServerAccountEntitlement : IWritable
{
    [UInt(32)] public EntitlementType EntitlementId { get; set; }
    [UInt(32)] public uint Count { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(EntitlementId, 32u);
        writer.Write(Count);
    }
}
