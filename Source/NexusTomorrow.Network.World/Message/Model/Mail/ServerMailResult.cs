using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.ServerMailResult, Direction.ServerToClient)]
public sealed partial class ServerMailResult
{
    // actions 2 and 3 invoke RefreshMail LUA event
    [UInt(32)] public uint Action { get; set; } // TODO: Analyze sniffs for possible values
    [UInt(64)] public ulong MailId { get; set; }
    [UInt(8)] public GenericError Result { get; set; }
}
