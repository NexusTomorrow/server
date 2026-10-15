using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildInfoMessageUpdate, Direction.ServerToClient)]
public sealed partial class ServerGuildInfoMessageUpdate
{
    public Identity GuildIdentity { get; set; }
    [WideString] public string InfoMessage { get; set; }
}
