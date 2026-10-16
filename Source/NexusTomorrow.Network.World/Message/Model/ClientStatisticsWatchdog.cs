using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client023C, Direction.ClientToServer)]
public sealed partial class Client023C
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [Float32] public float Unknown3 { get; private set; }
    [Float32] public float Unknown4 { get; private set; }
    [Float32] public float Unknown5 { get; private set; }
    [UInt(32)] public uint Unknown6 { get; private set; }
}
