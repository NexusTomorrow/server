using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0769, Direction.ServerToClient)]
public sealed partial class GuildRecruitmentDescriptionChanged
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [WideString] public string Unknown1 { get; set; } = "";
}
