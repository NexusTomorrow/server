using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Server05A1, Direction.ServerToClient)]
public sealed partial class Server05A1
{
    [WireStruct]
    public sealed partial class Server05A1Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server05A1Part0> Unknown0 { get; } = [];
}
