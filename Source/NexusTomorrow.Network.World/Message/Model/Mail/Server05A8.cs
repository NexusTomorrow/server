using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Server05A8, Direction.ServerToClient)]
public sealed partial class Server05A8
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(8)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
