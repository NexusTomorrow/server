using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[Message(GameMessageOpcode.ClientSetIgnoreDuelRequests)]
[WireMessage(GameMessageOpcode.ClientSetIgnoreDuelRequests, Direction.ClientToServer)]
public sealed partial class ClientSetIgnoreDuelRequests : IReadable
{
    [UInt(1)] public bool Ignore { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Ignore = reader.ReadBit();
    }
}
