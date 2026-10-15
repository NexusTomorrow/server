using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildFlagsUpdate, Direction.ServerToClient)]
public sealed partial class ServerGuildFlagsUpdate
{
    public Identity GuildIdentity { get; set; }
    [UInt(32)] public GuildFlag Flags { get; set; }
}
