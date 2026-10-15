using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Option;

// If sent without a CharacterId, applies to all characters on the account
[WireMessage(GameMessageOpcode.BiInputKeySet, Direction.Both)]
public sealed partial class BiInputKeySet
{
    [Array(CountBits = 32)] public List<Binding> Bindings { get; set; } = [];
    [UInt(64)] public ulong CharacterId { get; set; }
}
