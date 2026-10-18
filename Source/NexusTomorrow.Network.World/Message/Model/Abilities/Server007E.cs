using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server007E, Direction.ServerToClient)]
public sealed partial class Server007E
{
    [WireStruct]
    public sealed partial class Server007EPart0
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(64)] public ulong Unknown1 { get; set; }
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server007EPart0> Unknown1 { get; } = [];
    [UInt(32)] public uint Unknown2 { get; set; }
}
