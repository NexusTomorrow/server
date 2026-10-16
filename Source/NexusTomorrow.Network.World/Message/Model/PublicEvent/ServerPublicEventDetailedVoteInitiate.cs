using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server06FE, Direction.ServerToClient)]
public sealed partial class Server06FE
{
    [WireStruct]
    public sealed partial class Server06FEPart0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(14)] public uint Unknown1 { get; set; }
    [UInt(14)] public ushort Unknown2 { get; set; }
    [Array(CountBits = 32)] public List<Server06FEPart0> Unknown3 { get; set; } = [];
    [UInt(1)] public bool Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
}
