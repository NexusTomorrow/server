using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientAccountItemGiftByIdentity)]
[WireMessage(GameMessageOpcode.ClientAccountItemGiftByIdentity, Direction.ClientToServer)]
public sealed partial class ClientAccountItemGiftByIdentity : IReadable
{
    [WideString] public string TransactionId { get; private set; }
    public Identity Recipient { get; private set; } = new();

    public void Read(GamePacketReader reader)
    {
        TransactionId = reader.ReadWideString();
        Recipient.Read(reader);
    }
}
