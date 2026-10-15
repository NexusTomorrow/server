using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerUnitGuildNameplateChanged)]
public class ServerUnitGuildNameplateChanged : IWritable
{
    public uint UnitId { get; set; }
    public string GuildName { get; set; }
    public GuildType GuildType { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.WriteStringWide(GuildName);
        writer.Write(GuildType, 4u);
    }
}
