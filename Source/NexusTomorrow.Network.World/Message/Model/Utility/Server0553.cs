using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Server0553, Direction.ServerToClient)]
public sealed partial class Server0553
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 5)] public List<Server0111Part1> Unknown1 { get; set; } = [];
}
