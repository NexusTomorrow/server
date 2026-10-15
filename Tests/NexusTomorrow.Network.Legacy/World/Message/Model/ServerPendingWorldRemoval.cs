using NexusTomorrow.Game.Static.Map;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPendingWorldRemoval)]
public class ServerPendingWorldRemoval : IWritable
{
    public WorldRemovalReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Reason, 3u);
    }
}
