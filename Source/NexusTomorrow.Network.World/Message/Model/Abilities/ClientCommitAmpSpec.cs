using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Submits the currently selected AMPs so they are saved to the player's current spec.
[WireMessage(GameMessageOpcode.ClientCommitAmpSpec, Direction.ClientToServer)]
public sealed partial class ClientCommitAmpSpec
{
    [Array(CountBits = 7), UInt(16)] public List<ushort> Amps { get; } = [];
}
