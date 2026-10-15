using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountItemCooldownSet)]
[WireMessage(GameMessageOpcode.ServerAccountItemCooldownSet, Direction.ServerToClient)]
public sealed partial class ServerAccountItemCooldownSet : IWritable
{
    public AccountItemCooldown Cooldown { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Cooldown.Write(writer);
    }
}
