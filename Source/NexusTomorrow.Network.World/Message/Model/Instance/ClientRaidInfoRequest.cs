using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[Message(GameMessageOpcode.ClientRaidInfoRequest)]
[WireMessage(GameMessageOpcode.ClientRaidInfoRequest, Direction.ClientToServer)]
public sealed partial class ClientRaidInfoRequest : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
