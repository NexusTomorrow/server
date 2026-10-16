using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Server05A2, Direction.ServerToClient)]
public sealed partial class Server05A2
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }
}
