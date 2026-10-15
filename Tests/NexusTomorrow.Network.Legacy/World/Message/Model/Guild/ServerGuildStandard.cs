using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildStandard)]
public class ServerGuildStandard : IWritable
{
    public Identity GuildIdentity { get; set; }
    public GuildStandard GuildStandard { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        GuildStandard.Write(writer);
    }
}
