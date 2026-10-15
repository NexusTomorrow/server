using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientAccountItemGiftReturn)]
[WireMessage(GameMessageOpcode.ClientAccountItemGiftReturn, Direction.ClientToServer)]
public sealed partial class ClientAccountItemGiftReturn : IReadable
{
    [WideString] public string TransactionId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        TransactionId = reader.ReadWideString();
    }
}
