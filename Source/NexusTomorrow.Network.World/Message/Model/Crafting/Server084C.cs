using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Server084C, Direction.ServerToClient)]
public sealed partial class Server084C
{
    [WireStruct]
    public sealed partial class Server084CPart0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [Float32] public float Unknown4 { get; set; }
        [Float32] public float Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server084CPart0> Unknown0 { get; set; } = [];
}
