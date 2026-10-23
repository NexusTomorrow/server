using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client076E, Direction.ClientToServer)]
public sealed partial class GuildRecruitmentRequestDetails
{
    public RealmScopedId Unknown0 { get; private set; } = new();
}
