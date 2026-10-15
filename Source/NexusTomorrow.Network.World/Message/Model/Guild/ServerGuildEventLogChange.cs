using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Can add a new entry or change an existing entry; the entry's own GuildIdentity names the guild
[WireMessage(GameMessageOpcode.ServerGuildEventLogChange, Direction.ServerToClient)]
public sealed partial class ServerGuildEventLogChange
{
    public GuildEventLog LogEntry { get; set; }
}
