using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[Message(GameMessageOpcode.ClientCheat)]
public class ClientCheat : IReadable
{
    public string Message { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Message = reader.ReadWideString();
    }
}
