using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Who.Parameter;

namespace NexusTomorrow.Network.World.Message.Model.Who;

[WireMessage(GameMessageOpcode.Client0959, Direction.ClientToServer)]
public sealed partial class Client0959
{
    [Array(CountBits = 32)] public List<Client0959Part0> Unknown0 { get; private set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown1 { get; private set; } = [];
}
