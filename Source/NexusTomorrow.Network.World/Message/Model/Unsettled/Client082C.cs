using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client082C, Direction.ClientToServer)]
public sealed partial class Client082C
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [WideString] public string Unknown3 { get; set; } = "";
    [UInt(64)] public ulong Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
}
