using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Not seen in sniffs. Seems that ServerGuildInit and ServerGuildJoin initialise a standard set of 10 guild bank tabs.
// In theory seems you can have more than 10 guild bank tabs using this.
[WireMessage(GameMessageOpcode.ServerGuildBankTabCount, Direction.ServerToClient)]
public sealed partial class ServerGuildBankTabCount
{
    public Identity GuildIdentity { get; set; }
    [UInt(32)] public uint Count { get; set; }
}
