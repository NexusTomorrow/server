using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ServerMailTakeAttachment, Direction.ServerToClient)]
public sealed partial class ServerMailTakeAttachment
{
    [UInt(64)] public ulong MailId { get; set; }
    [UInt(8)] public GenericError Result { get; set; }
    [UInt(32)] public uint Index { get; set; }
}
