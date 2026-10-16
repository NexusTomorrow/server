using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server01B2, Direction.ServerToClient)]
public sealed partial class Server01B2
{
    [WireStruct]
    public sealed partial class Server01B2Part0
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
        [UInt(16)] public ushort Unknown1 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server01B2Part0> Unknown0 { get; } = [];
}
