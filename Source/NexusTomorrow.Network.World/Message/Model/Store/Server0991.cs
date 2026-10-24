using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Store;

[WireMessage(GameMessageOpcode.Server0991, Direction.ServerToClient)]
public sealed partial class Server0991
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
    [WideString] public string Unknown3 { get; set; } = "";
    [WideString] public string Unknown4 { get; set; } = "";
    [WideString] public string Unknown5 { get; set; } = "";
    [WideString] public string Unknown6 { get; set; } = "";
    [WideString] public string Unknown7 { get; set; } = "";
    [WideString] public string Unknown8 { get; set; } = "";
    [WideString] public string Unknown9 { get; set; } = "";
    [Float32] public float Unknown10 { get; set; }
    [Float32] public float Unknown11 { get; set; }
    [Float32] public float Unknown12 { get; set; }
    [WideString] public string Unknown13 { get; set; } = "";
}
