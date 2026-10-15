using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ClientWarPartyBossTokensRequest, Direction.ClientToServer)]
public sealed partial class ClientWarPartyBossTokensRequest
{
    public Identity GuildIdentity { get; private set; } = new(); // guild to request token info for
}
