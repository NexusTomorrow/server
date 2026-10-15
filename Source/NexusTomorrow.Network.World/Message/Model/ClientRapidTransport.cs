using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientRapidTransport)]
[WireMessage(GameMessageOpcode.ClientRapidTransport, Direction.ClientToServer)]
public sealed partial class ClientRapidTransport : IReadable
{
    [UInt(14)] public ushort TaxiNode { get; private set; }
    // not sure about the name - some increment
    [UInt(32)] public uint Time { get; private set; }

    public void Read(GamePacketReader reader)
    {
        TaxiNode = reader.ReadUShort(14u);
        Time = reader.ReadUInt();
    }
}
