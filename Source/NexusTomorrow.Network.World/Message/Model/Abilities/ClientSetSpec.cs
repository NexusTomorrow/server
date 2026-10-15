using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ClientSetSpec)]
[WireMessage(GameMessageOpcode.ClientSetSpec, Direction.ClientToServer)]
public sealed partial class ClientSetSpec : IReadable
{
    [UInt(3)] public byte SpecIndex { get; private set; }

    public void Read(GamePacketReader reader)
    {
        SpecIndex = reader.ReadByte(3u);
    }
}
