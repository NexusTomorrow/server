using NexusTomorrow.Game.Static.Mail;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ServerMailAvailable, Direction.ServerToClient)]
public sealed partial class ServerMailAvailable
{
    [WireStruct]
    public sealed partial class Attachment
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
    }

    [WireStruct]
    public sealed partial class Mail
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
    }

    [UInt(1)] public bool NewMail { get; set; } = true;
    [Array(CountBits = 32)] public List<Mail> MailList { get; set; } = [];
}
