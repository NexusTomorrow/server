using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerRecruitmentGuildDescription)]
public class ServerRecruitmentGuildDescription : IWritable
{
    public Identity GuildIdentity { get; set; }
    public string Description { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.WriteStringWide(Description);
    }
}
