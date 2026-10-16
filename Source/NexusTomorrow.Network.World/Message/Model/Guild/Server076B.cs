using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server076B, Direction.ServerToClient)]
public sealed partial class Server076B
{
    [WireStruct]
    public sealed partial class Server076BPart0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [WideString] public string Unknown1 { get; set; } = "";
        [WideString] public string Unknown2 { get; set; } = "";
        public Server0767Part1 Unknown3 { get; set; } = new();
    }

    [Array(CountBits = 32)] public List<Server076BPart0> Unknown0 { get; set; } = [];
}
