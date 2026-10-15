using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingRemoveItem)]
[WireMessage(GameMessageOpcode.ClientP2PTradingRemoveItem, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingRemoveItem : IReadable
{
    [UInt(32)] public ulong ItemGuid { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ItemGuid = reader.ReadUInt();
    }
}
