using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientEnteredWorld)]
[WireMessage(GameMessageOpcode.ClientEnteredWorld, Direction.ClientToServer)]
public sealed partial class ClientEnteredWorld : IReadable
{
    [UInt(15)] public ushort WorldZoneId { get; private set; } // 15

    public void Read(GamePacketReader reader)
    {
        WorldZoneId = reader.ReadUShort(15u);
    }
}
