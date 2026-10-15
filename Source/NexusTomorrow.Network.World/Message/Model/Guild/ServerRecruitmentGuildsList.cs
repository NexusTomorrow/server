using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerRecruitmentGuildsList)]
[WireMessage(GameMessageOpcode.ServerRecruitmentGuildsList, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildsList : IWritable
{
    [Array(CountBits = 32)] public List<RecruitmentGuildInfo> Guilds { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guilds.Count);
        Guilds.ForEach(guild => guild.Write(writer));
    }
}
