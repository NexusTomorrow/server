using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingAddItemToCrate)]
[WireMessage(GameMessageOpcode.ClientHousingAddItemToCrate, Direction.ClientToServer)]
public sealed partial class ClientHousingAddItemToCrate : IReadable
{
    [UInt(64)] public ulong ItemGuid { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ItemGuid = reader.ReadULong();
    }
}
