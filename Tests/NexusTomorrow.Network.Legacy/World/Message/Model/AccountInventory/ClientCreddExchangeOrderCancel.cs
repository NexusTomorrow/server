using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientCreddExchangeOrderCancel)]
public class ClientCreddExchangeOrderCancel : IReadable
{
    public Identity Unused { get; private set; } = new();
    public ulong CreddOrderId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Unused.Read(reader);
        CreddOrderId = reader.ReadULong();
    }
}
