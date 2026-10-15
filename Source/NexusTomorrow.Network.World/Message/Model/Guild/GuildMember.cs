using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class GuildMember
{
    public Identity PlayerIdentity { get; set; }
    [UInt(32)] public uint Rank { get; set; }
    [WideString] public string Name { get; set; }
    [UInt(2)] public Sex Sex { get; set; }
    [UInt(8)] public Class Class { get; set; }
    [Pad(24)] public Pad ClassHigh { get; }
    [UInt(8)] public Path Path { get; set; }
    [Pad(24)] public Pad PathHigh { get; }
    [UInt(32)] public uint Level { get; set; }
    [Float32] public float LastLogoutTimeDays { get; set; }
    [UInt(32)] public uint PvpWins { get; set; }
    [UInt(32)] public uint PvpLosses { get; set; }
    [UInt(32)] public uint PvpDraws { get; set; }
    [WideString] public string Note { get; set; }
    [UInt(32)] public uint RecruitmentAvailability { get; set; } // Only bit 2 is used
    [UInt(32)] public int CommunityReservedPlotIndex { get; set; } = -1;
}
