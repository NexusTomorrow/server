using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Fires whenever a guild's credits or influence are gained, withdrawn, or spent.
[Message(GameMessageOpcode.ServerGuildInfluenceAndMoney)]
[WireMessage(GameMessageOpcode.ServerGuildInfluenceAndMoney, Direction.ServerToClient)]
public sealed partial class ServerGuildInfluenceAndMoney : IWritable
{
    public Identity GuildIdentity { get; set; }
    [UInt(32)] public uint Influence { get; set; }
    [UInt(32)] public uint BonusInfluenceRemaining { get; set; }
    [UInt(64)] public ulong CashAmount { get; set; }
    [UInt(32)] public uint WarCoins { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(Influence);
        writer.Write(BonusInfluenceRemaining);
        writer.Write(CashAmount);
        writer.Write(WarCoins);
    }
}
