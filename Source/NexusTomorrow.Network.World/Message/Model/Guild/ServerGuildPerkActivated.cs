using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildPerkActivated, Direction.ServerToClient)]
public sealed partial class ServerGuildPerkActivated
{
    public Identity GuildIdentity { get; set; }
    [UInt(14)] public ushort GuildPerkId { get; set; }
    [Float32] public float DaysAgoActivated { get; set; }
}
