using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server097A, Direction.ServerToClient)]
public sealed partial class Server097A
{
    [WireStruct]
    public sealed partial class Server097APart0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(14)] public ushort Unknown1 { get; set; }
        [UInt(7)] public byte Unknown2 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server097APart0> Unknown0 { get; } = [];
}
