using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildEventLog, Direction.ServerToClient)]
public sealed partial class ServerGuildEventLog
{
    public Identity GuildIdentity { get; set; }
    [UInt(1)] public bool IsBankLog { get; set; } // true = is bank log information, false = regular guild log information
    [Array(CountBits = 32)] public List<GuildEventLog> LogEntry { get; set; }
}
