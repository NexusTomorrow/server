using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ClientWarPartyBossTokensRequest)]
[WireMessage(GameMessageOpcode.ClientWarPartyBossTokensRequest, Direction.ClientToServer)]
public sealed partial class ClientWarPartyBossTokensRequest : IReadable
{
    public Identity GuildIdentity { get; private set; } = new(); // guild to request token info for

    public void Read(GamePacketReader reader)
    {
        GuildIdentity.Read(reader);
    }
}
