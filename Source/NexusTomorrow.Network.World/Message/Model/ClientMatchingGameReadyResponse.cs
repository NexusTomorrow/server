using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingMatchReadyResponse)]
[WireMessage(GameMessageOpcode.ClientMatchingMatchReadyResponse, Direction.ClientToServer)]
public sealed partial class ClientMatchingGameReadyResponse : IReadable
{
    [UInt(1)] public bool Response { get; private set; } // decline = false, join = true

    public void Read(GamePacketReader reader)
    {
        Response = reader.ReadBit();
    }
}
