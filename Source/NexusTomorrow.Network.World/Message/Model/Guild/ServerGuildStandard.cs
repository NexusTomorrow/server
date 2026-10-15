using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildStandard, Direction.ServerToClient)]
public sealed partial class ServerGuildStandard
{
    public Identity GuildIdentity { get; set; }
    public GuildStandard GuildStandard { get; set; } = new();
}
