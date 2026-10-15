using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientEntitySelect)]
[WireMessage(GameMessageOpcode.ClientEntitySelect, Direction.ClientToServer)]
public sealed partial class ClientEntitySelect : IReadable
{
    [UInt(32)] public uint Guid { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Guid = reader.ReadUInt();
    }
}
