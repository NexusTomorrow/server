using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[Message(GameMessageOpcode.ClientFortuneStart)]
[WireMessage(GameMessageOpcode.ClientFortuneStart, Direction.ClientToServer)]
public sealed partial class ClientFortuneStart : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
