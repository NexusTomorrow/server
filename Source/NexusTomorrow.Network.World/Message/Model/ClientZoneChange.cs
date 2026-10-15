using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientZoneChange)]
[WireMessage(GameMessageOpcode.ClientZoneChange, Direction.ClientToServer)]
public sealed partial class ClientZoneChange : IReadable
{
    [UInt(15)] public ushort PreviousZoneId { get; private set; }
    [UInt(15)] public ushort NewZoneId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PreviousZoneId = reader.ReadUShort(15u);
        NewZoneId = reader.ReadUShort(15u);
    }
}
