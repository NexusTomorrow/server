using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class GuildWithdrawlInfo : IWritable
{
    [UInt(64)] public ulong BankMoneyWithdrawnToday { get; set; }
    [FixedArray(10), UInt(32)] public uint[] BankTabWithdrawCount { get; set; } = new uint[10];
    [UInt(64)] public ulong BankRepairMoneyToday { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(BankMoneyWithdrawnToday);
        foreach (uint bankTab in BankTabWithdrawCount)
            writer.Write(bankTab);
        writer.Write(BankRepairMoneyToday);
    }
}
