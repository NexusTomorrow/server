using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildPerkUnlocked, Direction.ServerToClient)]
public sealed partial class ServerGuildPerkUnlocked
{
    public Identity GuildIdentity { get; set; }
    [UInt(14)] public ushort GuildPerkId { get; set; }
}
