using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ClientRecruitmentGuildGetDetailedGuildInfo, Direction.ClientToServer)]
public sealed partial class ClientRecruitmentGuildGetDetailedGuildInfo
{
    public Identity GuildIdentity { get; private set; } = new();
}
