using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountCurrencyGrant)]
[WireMessage(GameMessageOpcode.ServerAccountCurrencyGrant, Direction.ServerToClient)]
public sealed partial class ServerAccountCurrencyGrant : IWritable
{
    public AccountCurrency AccountCurrency { get; set; }
    [UInt(64)] public ulong NewAmount { get; set; }
    [UInt(64)] public ulong SignatureBonusAmount { get; set; }
    [UInt(64)] public ulong EssenceBonusAmount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        AccountCurrency.Write(writer);
        writer.Write(NewAmount);
        writer.Write(SignatureBonusAmount);
        writer.Write(EssenceBonusAmount);
    }
}
