using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildClassification, Direction.ServerToClient)]
public sealed partial class ServerGuildClassification
{
    public Identity GuildIdentity { get; set; }
    [UInt(3)] public GuildClassification Classification { get; set; }
}
