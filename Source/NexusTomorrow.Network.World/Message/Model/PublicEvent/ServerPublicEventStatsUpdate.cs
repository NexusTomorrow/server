using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server0130, Direction.ServerToClient)]
public sealed partial class Server0130
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server00D6Part1> Unknown1 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server00D6Part2> Unknown2 { get; set; } = [];
}
