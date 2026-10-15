using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientTradeskillPickTalent)]
[WireMessage(GameMessageOpcode.ClientTradeskillPickTalent, Direction.ClientToServer)]
public sealed partial class ClientTradeskillPickTalent : IReadable
{
    [UInt(32)] public TradeskillType TradeskillId { get; private set; }
    [UInt(32)] public uint Tier { get; private set; }
    [UInt(32)] public uint TradeskillBonusId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        TradeskillId = reader.ReadEnum<TradeskillType>(32u);
        Tier = reader.ReadUInt();
        TradeskillBonusId = reader.ReadUInt();
    }
}
