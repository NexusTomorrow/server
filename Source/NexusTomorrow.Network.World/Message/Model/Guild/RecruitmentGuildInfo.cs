using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class RecruitmentGuildInfo : IWritable
{
    [UInt(64)] public ulong GuildId { get; set; }
    [WideString] public string GuildName { get; set; }
    [WideString] public string GuildMasterName { get; set; }
    public GuildStats Stats { get; set; }
    [Array(CountBits = 32), WideString] public List<string> Recruiters { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GuildId);
        writer.WriteStringWide(GuildName);
        writer.WriteStringWide(GuildMasterName);
        Stats.Write(writer);

        writer.Write(Recruiters.Count);
        Recruiters.ForEach(s => writer.WriteStringWide(s));
    }
}
