using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

// Initiates a spell cast to unstick the player
[WireMessage(GameMessageOpcode.ClientStuck, Direction.ClientToServer)]
public sealed partial class ClientStuck
{
    [UInt(3)] public UnstickType UnstickingType { get; private set; }
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }
}
