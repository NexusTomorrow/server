using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientResurrectRequest)]
[WireMessage(GameMessageOpcode.ClientResurrectRequest, Direction.ClientToServer)]
public sealed partial class ClientResurrectRequest : IReadable
{
    public void Read(GamePacketReader reader)
    {
    }
}
