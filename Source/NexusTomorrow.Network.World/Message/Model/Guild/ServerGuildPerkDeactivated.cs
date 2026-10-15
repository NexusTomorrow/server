using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildPerkDeactivated, Direction.ServerToClient)]
public sealed partial class ServerGuildPerkDeactivated
{
    public Identity GuildIdentity { get; set; }
    [UInt(14)] public ushort GuildPerkId { get; set; }
}
