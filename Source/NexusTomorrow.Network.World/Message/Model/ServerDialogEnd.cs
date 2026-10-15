using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Not captured in any sniffs so maybe was not needed
[Message(GameMessageOpcode.ServerDialogEnd)]
[WireMessage(GameMessageOpcode.ServerDialogEnd, Direction.ServerToClient)]
public sealed partial class ServerDialogEnd : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // zero byte message
    }
}
