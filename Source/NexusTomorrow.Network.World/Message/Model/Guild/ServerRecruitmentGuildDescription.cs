using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerRecruitmentGuildDescription, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildDescription
{
    public Identity GuildIdentity { get; set; }
    [WideString] public string Description { get; set; }
}
