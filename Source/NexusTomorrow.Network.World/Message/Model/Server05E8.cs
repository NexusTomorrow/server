using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server05E8, Direction.ServerToClient)]
public sealed partial class Server05E8
{
    [UInt(2)] public byte Unknown0 { get; set; }
    public Server05E8Part0 Unknown1 { get; set; } = new();
}
