using NexusTomorrow.Game.Static.Map;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPendingWorldRemoval)]
[WireMessage(GameMessageOpcode.ServerPendingWorldRemoval, Direction.ServerToClient)]
public sealed partial class ServerPendingWorldRemoval : IWritable
{
    [UInt(3)] public WorldRemovalReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Reason, 3u);
    }
}
