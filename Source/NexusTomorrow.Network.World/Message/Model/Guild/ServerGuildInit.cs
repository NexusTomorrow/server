using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildInit)]
[WireMessage(GameMessageOpcode.ServerGuildInit, Direction.ServerToClient)]
public sealed partial class ServerGuildInit : IWritable
{
    [UInt(32), Count] public uint GuildsCount { get; set; }
    [UInt(32)] public uint ShowNameplateIndex { get; set; } // Of the guilds in the Guilds, this is the list index of the guild the player will display on their nameplate
    [Array(CountFrom = nameof(GuildsCount))] public List<GuildData> Guilds { get; set; } = [];
    [Array(CountFrom = nameof(GuildsCount))] public List<GuildMember> Self { get; set; } = [];
    [Array(CountFrom = nameof(GuildsCount))] public List<GuildWithdrawlInfo> BankWithdrawlInfo { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guilds.Count);
        writer.Write(ShowNameplateIndex);
        Guilds.ForEach(w => w.Write(writer));
        Self.ForEach(w => w.Write(writer));
        BankWithdrawlInfo.ForEach(w => w.Write(writer));
    }
}
