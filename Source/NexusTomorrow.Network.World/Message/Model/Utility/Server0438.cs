using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Server0438, Direction.ServerToClient)]
public sealed partial class Server0438
{
    [WireStruct]
    public sealed partial class Server0438Part0
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(64)] public ulong Unknown1 { get; set; }
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32), Count] public uint Unknown2 { get; set; }
    [Array(CountFrom = nameof(Unknown2))] public List<Server0438Part0> Unknown3 { get; } = [];
    [Array(CountFrom = nameof(Unknown2)), UInt(32)] public List<uint> Unknown4 { get; } = [];
}
