using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Functionally the same as ClientGuildStandardModify
[Message(GameMessageOpcode.ClientGuildSetStandard)]
[WireMessage(GameMessageOpcode.ClientGuildSetStandard, Direction.ClientToServer)]
public sealed partial class ClientGuildSetStandard : IReadable
{
    public Identity GuildIdentity { get; private set; } = new();
    public GuildStandard GuildStandard { get; private set; } = new();

    public void Read(GamePacketReader reader)
    {
        GuildIdentity.Read(reader);
        GuildStandard.Read(reader);
    }
}
