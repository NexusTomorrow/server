using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server01A0, Direction.ServerToClient)]
public sealed partial class Server01A0
{
    [WireStruct]
    public sealed partial class Server01A0Part0
    {
        [UInt(18)] public uint Unknown0 { get; set; }
        [UInt(4)] public byte Unknown1 { get; set; }
        [UInt(3)] public byte Unknown2 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server01A0Part0> Unknown0 { get; set; } = [];
}
