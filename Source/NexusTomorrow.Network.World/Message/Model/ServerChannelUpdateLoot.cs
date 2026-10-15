using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerChannelUpdateLoot)]
[WireMessage(GameMessageOpcode.ServerChannelUpdateLoot, Direction.ServerToClient)]
public sealed partial class ServerChannelUpdateLoot : IWritable
{
    [UInt(4)] public CurrencyType CurrencyId { get; set; }
    [UInt(64)] public ulong Amount { get; set; }
    [UInt(64)] public ulong SignatureBonus { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CurrencyId, 4u);
        writer.Write(Amount);
        writer.Write(SignatureBonus);
    }
}
