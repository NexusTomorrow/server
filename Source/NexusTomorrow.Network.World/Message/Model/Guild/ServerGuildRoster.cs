using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildRoster, Direction.ServerToClient)]
public sealed partial class ServerGuildRoster
{
    public Identity GuildIdentity { get; set; }
    [Array(CountBits = 32)] public List<GuildMember> GuildMembers { get; set; } = [];
    [UInt(1)] public bool Done { get; set; } = true; // Indicates if the roster is complete (true) or if more members are expected (false)
}
