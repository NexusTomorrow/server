using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountItemCooldowns)]
[WireMessage(GameMessageOpcode.ServerAccountItemCooldowns, Direction.ServerToClient)]
public sealed partial class ServerAccountItemCooldowns : IWritable
{
    [Array(CountBits = 32)] public List<AccountItemCooldown> AccountItemCooldowns { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountItemCooldowns.Count);
        AccountItemCooldowns.ForEach(cooldown => cooldown.Write(writer));
    }
}
