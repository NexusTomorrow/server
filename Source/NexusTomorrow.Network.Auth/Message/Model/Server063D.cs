using NexusTomorrow.Network.Auth.Static;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.Server063D, Direction.ServerToClient)]
public sealed partial class Server063D
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
}
