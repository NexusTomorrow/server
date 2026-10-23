using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// A whole guild, as the client takes one on: the record the guild's own update messages and GuildTypeLib's getters work on.
/// </summary>
[WireStruct]
public sealed partial class GuildRecord
{
    /// <summary>
    /// The guild, by its id within the realm.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// The guild's name.
    /// </summary>
    [WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The guild's flags, a bit each as GuildTypeLib's SetFlags sets them.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }

    /// <summary>
    /// The kind of guild, a GuildType value.
    /// </summary>
    [UInt(4)] public byte Type { get; set; }

    /// <summary>
    /// The guild's ten ranks, in rank order.
    /// </summary>
    [FixedArray(10)] public GuildRank[] Ranks { get; set; } = [.. Enumerable.Range(0, 10).Select(_ => new GuildRank())];

    /// <summary>
    /// The guild's standard, laid out as a holomark.
    /// </summary>
    public GuildHolomark Standard { get; set; } = new();

    /// <summary>
    /// How many members the guild has.
    /// </summary>
    [UInt(32)] public uint MemberCount { get; set; }

    /// <summary>
    /// How many of them are online.
    /// </summary>
    [UInt(32)] public uint OnlineMemberCount { get; set; }

    /// <summary>
    /// The guild's influence.
    /// </summary>
    [UInt(32)] public uint Influence { get; set; }

    /// <summary>
    /// The bonus influence the guild can still earn.
    /// </summary>
    [UInt(32)] public uint BonusInfluenceRemaining { get; set; }

    /// <summary>
    /// The money in the guild's bank.
    /// </summary>
    [UInt(64)] public ulong Money { get; set; }

    /// <summary>
    /// The guild's war coins.
    /// </summary>
    [UInt(32)] public uint WarCoins { get; set; }

    /// <summary>
    /// How many bank tabs the guild has.
    /// </summary>
    [UInt(32)] public uint BankTabCount { get; set; }

    /// <summary>
    /// Each bank tab's name, in tab order.
    /// </summary>
    [FixedArray(10), WideString] public string[] BankTabNames { get; set; } = [.. Enumerable.Repeat(string.Empty, 10)];

    /// <summary>
    /// The perks the guild has unlocked, a bit per perk id.
    /// </summary>
    [FixedArray(16), UInt(8)] public byte[] UnlockedPerks { get; set; } = new byte[16];

    /// <summary>
    /// The perks active now.
    /// </summary>
    [Array(CountBits = 32)] public List<GuildActivePerk> ActivePerks { get; set; } = [];

    /// <summary>
    /// The guild's messages, queue state, PvP record and creation date.
    /// </summary>
    public GuildStatus Status { get; set; } = new();
}
