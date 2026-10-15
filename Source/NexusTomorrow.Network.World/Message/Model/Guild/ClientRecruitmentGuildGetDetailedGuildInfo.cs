using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ClientRecruitmentGuildGetDetailedGuildInfo)]
[WireMessage(GameMessageOpcode.ClientRecruitmentGuildGetDetailedGuildInfo, Direction.ClientToServer)]
public sealed partial class ClientRecruitmentGuildGetDetailedGuildInfo : IReadable
{
    public Identity GuildIdentity { get; private set; } = new();

    public void Read(GamePacketReader reader)
    {
        GuildIdentity.Read(reader);
    }
}
