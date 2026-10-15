using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildClassification)]
public class ServerGuildClassification : IWritable
{
    public Identity GuildIdentity { get; set; }
    public GuildClassification Classification { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(Classification, 14u);
    }
}
