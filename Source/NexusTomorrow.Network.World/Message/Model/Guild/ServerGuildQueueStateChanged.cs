using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Fires whenever the arena team or warparty enters a queue, leaves a queue, enters a match, or leaves a match.
[WireMessage(GameMessageOpcode.ServerGuildQueueStateChanged, Direction.ServerToClient)]
public sealed partial class ServerGuildQueueStateChanged
{
    public Identity GuildIdentity { get; set; }
    [UInt(3)] public GuildQueueState State { get; set; }
}
