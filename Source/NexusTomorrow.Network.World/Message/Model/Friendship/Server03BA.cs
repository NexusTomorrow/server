using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03BA, Direction.ServerToClient)]
public sealed partial class Server03BA
{
    [WireStruct]
    public sealed partial class Server03BAPart0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        public Client00B5Part0 Unknown1 { get; set; } = new();
        [UInt(3)] public uint Unknown2 { get; set; }
        [Float32] public float Unknown3 { get; set; }
        [WideString] public string Unknown4 { get; set; } = "";
        [WideString] public string Unknown5 { get; set; } = "";
        [UInt(8)] public byte Unknown6 { get; set; }
        [UInt(6)] public byte Unknown7 { get; set; }
        [UInt(3)] public byte Unknown8 { get; set; }
        [UInt(8)] public byte Unknown9 { get; set; }
    }

    [Array(CountBits = 16)] public List<Server03BAPart0> Unknown0 { get; set; } = [];
}
