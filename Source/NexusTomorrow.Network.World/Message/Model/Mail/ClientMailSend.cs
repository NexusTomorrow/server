using NexusTomorrow.Game.Static.Mail;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ClientMailSend)]
[WireMessage(GameMessageOpcode.ClientMailSend, Direction.ClientToServer)]
public sealed partial class ClientMailSend : IReadable
{
    [WideString] public string Name { get; private set; }
    [WideString] public string Realm { get; private set; }
    [WideString] public string Subject { get; private set; }
    [WideString] public string Message { get; private set; }
    [UInt(64)] public ulong CreditsSent { get; private set; }
    [UInt(64)] public ulong CashOnDeliveryAmount { get; private set; }
    [UInt(2)] public DeliverySpeed DeliverySpeed { get; private set; }
    [UInt(32)] public uint MailboxUnitId { get; private set; }
    [FixedArray(10), UInt(64)] public ulong[] Items { get; private set; } = new ulong[10];

    public void Read(GamePacketReader reader)
    {
        Name = reader.ReadWideString();
        Realm = reader.ReadWideString();
        Subject = reader.ReadWideString();
        Message = reader.ReadWideString();
        CreditsSent = reader.ReadULong();
        CashOnDeliveryAmount = reader.ReadULong();
        DeliverySpeed = reader.ReadEnum<DeliverySpeed>(2u);
        MailboxUnitId = reader.ReadUInt();

        for (uint i = 0; i < 10; i++)
            Items[i] = reader.ReadULong();
    }
}
