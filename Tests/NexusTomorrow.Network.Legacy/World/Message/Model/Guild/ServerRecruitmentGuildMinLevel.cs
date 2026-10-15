using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerRecruitmentGuildMinLevel)]
public class ServerRecruitmentGuildMinLevel : IWritable
{
    public Identity GuildIdentity { get; set; }
    public uint MinimumLevel { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(MinimumLevel);
    }
}
