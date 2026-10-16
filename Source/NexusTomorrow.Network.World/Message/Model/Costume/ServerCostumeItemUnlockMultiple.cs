using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Server0258, Direction.ServerToClient)]
public sealed partial class Server0258
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown1 { get; set; } = [];
}
