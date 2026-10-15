using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Fires whenever the arena team or warparty enters a queue, leaves a queue, enters a match, or leaves a match.
[Message(GameMessageOpcode.ServerGuildQueueStateChanged)]
[WireMessage(GameMessageOpcode.ServerGuildQueueStateChanged, Direction.ServerToClient)]
public sealed partial class ServerGuildQueueStateChanged : IWritable
{
    public Identity GuildIdentity { get; set; }
    [UInt(3)] public GuildQueueState State { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(State, 3u);
    }
}
