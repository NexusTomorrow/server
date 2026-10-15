using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class GuildRank : IWritable
{
    [WideString] public string RankName { get; set; } = "";
    [UInt(32)] public GuildRankPermission PermissionMask { get; set; } = GuildRankPermission.Disabled;
    [UInt(64)] public ulong BankWithdrawalPermissions { get; set; }
    [UInt(64)] public ulong MoneyWithdrawalLimit { get; set; }
    [UInt(64)] public ulong RepairLimit { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(RankName);
        writer.Write(PermissionMask, 32u);
        writer.Write(BankWithdrawalPermissions);
        writer.Write(MoneyWithdrawalLimit);
        writer.Write(RepairLimit);
    }
}
