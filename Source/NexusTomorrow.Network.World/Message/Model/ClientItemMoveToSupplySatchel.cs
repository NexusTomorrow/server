using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientItemMoveToSupplySatchel)]
[WireMessage(GameMessageOpcode.ClientItemMoveToSupplySatchel, Direction.ClientToServer)]
public sealed partial class ClientItemMoveToSupplySatchel : IReadable
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [UInt(32)] public uint Amount { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ItemGuid = reader.ReadULong();
        Amount = reader.ReadUInt();
    }
}
