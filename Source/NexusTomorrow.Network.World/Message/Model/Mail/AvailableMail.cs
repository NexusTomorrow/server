using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

/// <summary>
/// Adds mails to the client's mailbox; a mail it already holds is left as it is. If it added any, the client raises its AvailableMail
/// event.
/// </summary>
[WireMessage(GameMessageOpcode.Message05A3, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class AvailableMail
{
    [WireStruct]
    public sealed partial class Message05A3Part1
    {
        [UInt(18)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(64)] public ulong Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(64)] public ulong Unknown5 { get; set; }
        [UInt(18)] public uint Unknown6 { get; set; }
        [FixedArray(5), UInt(32)] public uint[] Unknown7 { get; set; } = new uint[5];
        [FixedArray(8), UInt(32)] public uint[] Unknown8 { get; set; } = new uint[8];
    }

    [WireStruct]
    public sealed partial class Message05A3Part0
    {
        /// <summary>
        /// The mail's id.
        /// </summary>
        [UInt(64)] public ulong MailId { get; set; }

        /// <summary>
        /// The kind of sender, as MailSystemLib GetSenderType returns it.
        /// </summary>
        [UInt(32)] public uint SenderType { get; set; }

        [UInt(14)] public ushort Unknown2 { get; set; }
        [WideString] public string Unknown3 { get; set; } = "";
        [WideString] public string Unknown4 { get; set; } = "";
        [UInt(21)] public uint Unknown5 { get; set; }
        [UInt(21)] public uint Unknown6 { get; set; }
        [UInt(18)] public uint Unknown7 { get; set; }
        [UInt(4)] public byte Unknown8 { get; set; }

        /// <summary>
        /// The amount MailSystemLib GetMessageInfo reports as monGift.
        /// </summary>
        [UInt(64)] public ulong GiftAmount { get; set; }

        /// <summary>
        /// The amount MailSystemLib GetMessageInfo reports as monCod.
        /// </summary>
        [UInt(64)] public ulong CodAmount { get; set; }

        /// <summary>
        /// The value the client turns into the mail's expiration time, which MailSystemLib GetExpirationTime reports.
        /// </summary>
        [Float32] public float Expiration { get; set; }

        /// <summary>
        /// Bit 0x2 marks the mail read; bit 0x8 marks a mail that cannot be returned.
        /// </summary>
        [UInt(32)] public uint Flags { get; set; }

        public RealmScopedId Unknown13 { get; set; } = new();
        [Array(CountBits = 32)] public List<Message05A3Part1> Unknown14 { get; set; } = [];
    }

    /// <summary>
    /// Passed to the AvailableMail event as bIsNewMail; when set, the client also raises its AlertMailInfo event for the unread mail it
    /// last added.
    /// </summary>
    [UInt(1)] public bool IsNewMail { get; set; }

    /// <summary>
    /// The mails.
    /// </summary>
    [Array(CountBits = 32)] public List<Message05A3Part0> Mails { get; set; } = [];
}
