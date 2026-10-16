using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server08BB, Direction.ServerToClient)]
public sealed partial class Server08BB
{
    [WireStruct]
    public sealed partial class Server08BBPart0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(3)] public byte Unknown1 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server08BBPart0> Unknown1 { get; set; } = [];
}
