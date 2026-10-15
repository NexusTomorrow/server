using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPendingWorldRemovalCancel)]
[WireMessage(GameMessageOpcode.ServerPendingWorldRemovalCancel, Direction.ServerToClient)]
public sealed partial class ServerPendingWorldRemovalCancel : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
