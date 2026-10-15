using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[Message(GameMessageOpcode.ClientCheat)]
[WireMessage(GameMessageOpcode.ClientCheat, Direction.ClientToServer)]
public sealed partial class ClientCheat : IReadable
{
    [WideString] public string Message { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Message = reader.ReadWideString();
    }
}
