using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.ClientCombatLogDisableOthers, Direction.ClientToServer)]
public sealed partial class ClientCombatLogDisableOthers
{
    // The flag is a 32-bit field: bit 0 is read as the flag and the 31 bits above it are skipped.
    [UInt(1)] public bool DisableOtherPlayers { get; set; }
    [Pad(31)] public Pad Reserved { get; }
}
