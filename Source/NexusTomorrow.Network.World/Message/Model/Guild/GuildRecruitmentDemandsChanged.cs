using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0768, Direction.ServerToClient)]
public sealed partial class GuildRecruitmentDemandsChanged
{
    public RealmScopedId Unknown0 { get; set; } = new();
    public GuildRecruitmentDemands Unknown1 { get; set; } = new();
}

