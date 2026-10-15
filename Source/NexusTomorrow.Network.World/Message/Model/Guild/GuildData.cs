using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class GuildData
{
    [WireStruct]
    public sealed partial class Info
    {
        [WireStruct]
        public sealed partial class PvPRating
        {
            [UInt(32)] public uint Wins { get; set; }
            [UInt(32)] public uint Losses { get; set; }
            [UInt(32)] public uint Draws { get; set; }
            [UInt(32)] public uint Rating { get; set; }
            [UInt(32)] public uint PvpSeason { get; set; }
            [UInt(32)] public uint KFactor { get; set; }
        }

        [WideString] public string MessageOfTheDay { get; set; }
        [WideString] public string GuildInfo { get; set; }
        [UInt(3)] public GuildQueueState QueueState { get; set; }
        public PvPRating PvPRatings { get; set; } = new();
        [Float32] public float GuildCreationDateInDays { get; set; }
    }

    [WireStruct]
    public sealed partial class ActivePerk
    {
        [UInt(14)] public GuildPerk Perk { get; set; } // Is the guildPerkId, see guildPerk tbl or GuildPerk enum
        [Float32] public float EndTime { get; set; }
    }

    [UInt(64)] public ulong GuildId { get; set; }
    [WideString] public string GuildName { get; set; }
    [UInt(32)] public GuildFlag Flags { get; set; }
    [UInt(4)] public GuildType Type { get; set; }

    [FixedArray(10)] public GuildRank[] Ranks { get; set; } = [.. Enumerable.Range(0, 10).Select(_ => new GuildRank())];

    public GuildStandard GuildStandard { get; set; } = new();

    [UInt(32)] public uint MemberCount { get; set; }
    [UInt(32)] public uint OnlineMemberCount { get; set; }
    [UInt(32)] public uint Influence { get; set; }
    [UInt(32)] public uint BonusInfluenceRemaining { get; set; }

    [UInt(64)] public ulong Money { get; set; }
    [UInt(32)] public uint WarCoins { get; set; }
    [UInt(32)] public uint BankTabCount { get; set; }
    [FixedArray(10), WideString] public string[] BankTabNames { get; set; } = [.. Enumerable.Repeat(string.Empty, 10)];

    [FixedArray(16), UInt(8)] public byte[] UnlockedPerks { get; set; } = new byte[16];
    // Set of flags represeting the unlock state of a GuildPerkId
    // GuildPerkId = 1 is bit 0, least significant bit of each byte first

    [Array(CountBits = 32)] public List<ActivePerk> ActivePerks { get; set; } = [];

    public Info GuildInfo { get; set; } = new();
}
