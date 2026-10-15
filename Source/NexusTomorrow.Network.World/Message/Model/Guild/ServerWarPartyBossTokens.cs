using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerWarPartyBossTokens, Direction.ServerToClient)]
public sealed partial class ServerWarPartyBossTokens
{
    public Identity GuildIdentity { get; set; }
    [Array(CountBits = 32)] public List<WarPartyBossToken> Tokens { get; set; } = [];
}
