using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[WireMessage(GameMessageOpcode.Server00AF, Direction.ServerToClient)]
public sealed partial class Server00AF
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server00AEPart0> Unknown1 { get; set; } = [];
}
