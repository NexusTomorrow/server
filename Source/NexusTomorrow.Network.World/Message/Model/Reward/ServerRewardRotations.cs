using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server07CA, Direction.ServerToClient)]
public sealed partial class Server07CA
{
    [WireStruct]
    public sealed partial class Server07CAPart0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(14)] public uint Unknown1 { get; set; }
        [Float32] public float Unknown2 { get; set; }
        [UInt(8)] public byte Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server07CAPart0> Unknown0 { get; set; }
}
