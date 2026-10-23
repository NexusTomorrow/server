using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server08B6, Direction.ServerToClient)]
public sealed partial class UnitSettlerImprovement
{
    [WireStruct]
    public sealed partial class Server08B6Part0
    {
        [WideString] public string Unknown0 { get; set; } = "";
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(14)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [Array(CountBits = 32), WideString] public List<string> Unknown3 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server08B6Part0> Unknown4 { get; set; } = [];
}
