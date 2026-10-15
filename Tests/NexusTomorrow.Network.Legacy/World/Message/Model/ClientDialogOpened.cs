using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientDialogOpened)]
public class ClientDialogOpened : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
