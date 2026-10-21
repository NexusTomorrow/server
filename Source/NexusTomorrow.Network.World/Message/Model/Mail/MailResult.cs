using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Server05A2, Direction.ServerToClient)]
public sealed partial class MailResult
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }
}
