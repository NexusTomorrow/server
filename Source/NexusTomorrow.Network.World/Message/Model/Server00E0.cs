using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server00E0, Direction.ServerToClient)]
public sealed partial class Server00E0
{
    [Array(CountBits = 32)] public List<Server00E0Part0> Unknown0 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server00E0Part0> Unknown1 { get; set; } = [];
}
