using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Server05A7, Direction.ServerToClient)]
public sealed partial class UnavailableMail
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
