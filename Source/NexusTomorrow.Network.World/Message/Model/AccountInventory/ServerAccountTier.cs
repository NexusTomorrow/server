using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountTier)]
[WireMessage(GameMessageOpcode.ServerAccountTier, Direction.ServerToClient)]
public sealed partial class ServerAccountTier : IWritable
{
    [UInt(5)] public AccountTier Tier { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Tier, 5u);
    }
}
