using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathScientistScanbotState, Direction.ServerToClient)]
public sealed partial class ServerPathScientistScanbotState
{
    [UInt(32)] public uint ScanbotUnitId { get; set; } // Sent with 0 to despawn the scanbot.
    [UInt(32)] public uint ScanbotCooldownMS { get; set; } // Only used when the scanbot is despawned.
}
