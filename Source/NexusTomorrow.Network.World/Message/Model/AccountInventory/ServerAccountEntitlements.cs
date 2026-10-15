using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountEntitlements)]
[WireMessage(GameMessageOpcode.ServerAccountEntitlements, Direction.ServerToClient)]
public sealed partial class ServerAccountEntitlements : IWritable
{
    [WireStruct]
    public sealed partial class AccountEntitlement : IWritable
    {
        [UInt(32)] public EntitlementType EntitlementId { get; set; }
        [UInt(32)] public uint Count { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(EntitlementId, 32u);
            writer.Write(Count);
        }
    }

    [Array(CountBits = 32)] public List<AccountEntitlement> AccountEntitlements { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountEntitlements.Count);
        AccountEntitlements.ForEach(e => e.Write(writer));
    }
}
