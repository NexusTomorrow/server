using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03BF, Direction.ClientToServer)]
public sealed partial class Client03BF
{
    [Array(CountBits = 8)] public List<Client00B5Part0> Unknown0 { get; private set; } = [];
    [Array(CountBits = 8), UInt(64)] public List<ulong> Unknown1 { get; private set; } = [];
}
