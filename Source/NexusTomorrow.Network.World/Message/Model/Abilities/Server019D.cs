using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server019D, Direction.ServerToClient)]
public sealed partial class Server019D
{
    [WireStruct]
    public sealed partial class Server019DPart0
    {
        [UInt(4)] public byte Unknown0 { get; set; }
        public ItemLocation Unknown1 { get; set; } = new();
        [UInt(32)] public uint Unknown2 { get; set; }
    }

    [UInt(3)] public byte Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(6)] public byte Unknown2 { get; set; }
    [Array(CountBits = 6)] public List<Server019DPart0> Unknown3 { get; set; } = [];
}
