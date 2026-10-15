using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildMotdUpdate, Direction.ServerToClient)]
public sealed partial class ServerGuildMotdUpdate
{
    public Identity GuildIdentity { get; set; }
    [WideString] public string MessageOfTheDay { get; set; }
}
