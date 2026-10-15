using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Functionally the same as ClientGuildStandardModify
[WireMessage(GameMessageOpcode.ClientGuildSetStandard, Direction.ClientToServer)]
public sealed partial class ClientGuildSetStandard
{
    public Identity GuildIdentity { get; private set; } = new();
    public GuildStandard GuildStandard { get; private set; } = new();
}
