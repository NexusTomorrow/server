using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// One of a guild's ranks, as GuildTypeLib's GetRanks reports it.
/// </summary>
[WireStruct]
public sealed partial class GuildRank
{
    /// <summary>
    /// The rank's name.
    /// </summary>
    [WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The rank's permissions, a bit each in the order GuildLib.GetPermissions lists them; GetRanks reports bit 0 inverted.
    /// </summary>
    [UInt(32)] public uint Permissions { get; set; }

    /// <summary>
    /// The rank's permissions on each bank tab, six bits a tab: whether it sees the tab, may deposit and needs the
    /// authenticator, and its withdrawal limit.
    /// </summary>
    [UInt(64)] public ulong BankTabPermissions { get; set; }

    /// <summary>
    /// How much money the rank may withdraw a day; 0x7FFFFFFFFFFFFFFF is no limit.
    /// </summary>
    [UInt(64)] public ulong MoneyWithdrawLimit { get; set; }

    /// <summary>
    /// How much the rank may spend on repairs a day; 0x7FFFFFFFFFFFFFFF is no limit.
    /// </summary>
    [UInt(64)] public ulong RepairLimit { get; set; }
}
