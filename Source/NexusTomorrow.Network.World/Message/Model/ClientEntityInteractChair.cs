using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientEntityInteractChair)]
[WireMessage(GameMessageOpcode.ClientEntityInteractChair, Direction.ClientToServer)]
public sealed partial class ClientEntityInteractChair : IReadable
{
    [UInt(32)] public uint ChairUnitId { get; private set; }
    [UInt(1)] public bool Remove { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ChairUnitId = reader.ReadUInt();
        Remove = reader.ReadBit();
    }
}
