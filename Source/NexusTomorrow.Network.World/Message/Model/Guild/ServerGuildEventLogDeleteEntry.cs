using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildEventLogDeleteEntry)]
[WireMessage(GameMessageOpcode.ServerGuildEventLogDeleteEntry, Direction.ServerToClient)]
public sealed partial class ServerGuildEventLogDeleteEntry : IWritable
{
    public Identity GuildIdentity { get; set; }
    [UInt(64)] public ulong EventLogId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(EventLogId);
    }
}
