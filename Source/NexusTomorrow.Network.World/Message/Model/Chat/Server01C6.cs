using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01C6, Direction.ServerToClient)]
public sealed partial class Server01C6
{
    public Server01B5Part0 Unknown0 { get; set; }
    [UInt(21)] public uint Unknown1 { get; set; }
    [UInt(21)] public uint Unknown2 { get; set; }
}
