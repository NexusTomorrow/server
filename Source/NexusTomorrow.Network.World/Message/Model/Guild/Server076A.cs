using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server076A, Direction.ServerToClient)]
public sealed partial class Server076A
{
    public Client00B5Part0 Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    public Server0768Part0 Unknown5 { get; set; }
}
