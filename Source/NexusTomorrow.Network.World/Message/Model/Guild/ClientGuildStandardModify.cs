using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Functionally the same as ClientGuildSetStandard
[Message(GameMessageOpcode.ClientGuildStandardModify)]
[WireMessage(GameMessageOpcode.ClientGuildStandardModify, Direction.ClientToServer)]
public sealed partial class ClientGuildStandardModify : IReadable
{
    public Identity GuildIdentity { get; private set; } = new();
    public GuildStandard GuildStandard { get; private set; } = new();

    public void Read(GamePacketReader reader)
    {
        GuildIdentity.Read(reader);
        GuildStandard.Read(reader);
    }
}
