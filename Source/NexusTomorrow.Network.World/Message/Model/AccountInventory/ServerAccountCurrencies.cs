using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountCurrencies)]
[WireMessage(GameMessageOpcode.ServerAccountCurrencies, Direction.ServerToClient)]
public sealed partial class ServerAccountCurrencies : IWritable
{
    [Array(CountBits = 32)] public List<AccountCurrency> AccountCurrencies { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountCurrencies.Count, 32u);

        AccountCurrencies.ForEach(a => a.Write(writer));
    }
}
