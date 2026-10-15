using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingAddItem)]
[WireMessage(GameMessageOpcode.ClientP2PTradingAddItem, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingAddItem : IReadable
{
    [UInt(64)] public ulong ItemGuid { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ItemGuid = reader.ReadULong();
    }
}
