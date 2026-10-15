using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientDialogOpened)]
[WireMessage(GameMessageOpcode.ClientDialogOpened, Direction.ClientToServer)]
public sealed partial class ClientDialogOpened : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
