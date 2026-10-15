using NexusTomorrow.Game.Static.Mail;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ServerMailAvailable)]
[WireMessage(GameMessageOpcode.ServerMailAvailable, Direction.ServerToClient)]
public sealed partial class ServerMailAvailable : IWritable
{
    [WireStruct]
    public sealed partial class Attachment : IWritable
    {
        [UInt(18)] public uint Item2Id { get; set; }
        [UInt(32)] public uint StackCount { get; set; }
        [UInt(32)] public uint Charges { get; set; }
        [UInt(64)] public ulong ItemRandomCircuitData { get; set; }
        [UInt(32)] public uint ItemRandomGlyphData { get; set; }
        [UInt(64)] public ulong ThresholdData { get; set; }
        [UInt(18)] public uint Unknown7 { get; set; }
        [FixedArray(5), UInt(32)] public uint[] Item2IdMicrochip { get; set; } = new uint[5];
        [FixedArray(8), UInt(32)] public uint[] Item2IdGlyph { get; set; } = new uint[8];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Item2Id, 18u);
            writer.Write(StackCount);
            writer.Write(Charges);
            writer.Write(ItemRandomCircuitData);
            writer.Write(ItemRandomGlyphData);
            writer.Write(ThresholdData);
            writer.Write(Unknown7, 18u);

            for (uint i = 0u; i < Item2IdMicrochip.Length; i++)
                writer.Write(Item2IdMicrochip[i]);

            for (uint i = 0u; i < Item2IdGlyph.Length; i++)
                writer.Write(Item2IdGlyph[i]);
        }
    }

    [WireStruct]
    public sealed partial class Mail : IWritable
    {
        [UInt(64)] public ulong MailId { get; set; }
        [UInt(32)] public SenderType SenderType { get; set; }
        [UInt(14)] public ContentType ContentType { get; set; } // Client uses this to format the mail content. More entries TBC
        [WideString] public string Subject { get; set; }
        [WideString] public string Message { get; set; }
        [UInt(21)] public uint SubjectStringId { get; set; } // LocalizedStringId. If set, Subject is ignored.
        [UInt(21)] public uint BodyStringId { get; set; } // LocalizedStringId. If set, Message is ignored.
        [UInt(18)] public uint FromCreatureId { get; set; }
        [UInt(4)] public byte CurrencySentType { get; set; }
        [UInt(64)] public ulong CurrencySentAmount { get; set; }
        [UInt(64)] public ulong CostOnDeliveryAmount { get; set; }
        [Float32] public float ExpiryTimeInDays { get; set; }
        [UInt(32)] public MailFlag Flags { get; set; }
        public Identity Sender { get; set; } = new();
        [Array(CountBits = 32)] public List<Attachment> Attachments { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(MailId);
            writer.Write(SenderType, 32);
            writer.Write(ContentType, 14);
            writer.WriteStringWide(Subject);
            writer.WriteStringWide(Message);
            writer.Write(SubjectStringId, 21);
            writer.Write(BodyStringId, 21);
            writer.Write(FromCreatureId, 18);
            writer.Write(CurrencySentType, 4);
            writer.Write(CurrencySentAmount);
            writer.Write(CostOnDeliveryAmount);
            writer.Write(ExpiryTimeInDays);
            writer.Write(Flags, 32);
            Sender.Write(writer);

            writer.Write(Attachments.Count);
            Attachments.ForEach(v => v.Write(writer));
        }
    }

    [UInt(1)] public bool NewMail { get; set; } = true;
    [Array(CountBits = 32)] public List<Mail> MailList { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(NewMail);

        writer.Write(MailList.Count);
        MailList.ForEach(v => v.Write(writer));
    }
}
