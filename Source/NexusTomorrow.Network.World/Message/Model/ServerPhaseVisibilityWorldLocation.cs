using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Most commonly checked against worldLocation2->phases for publicEventObjective
// Both properties used for group member visibility checks
// Used for Prerequisite checks of type = 0x18 and 0x19
[WireMessage(GameMessageOpcode.ServerPhaseVisibilityWorldLocation, Direction.ServerToClient)]
public sealed partial class ServerPhaseVisibilityWorldLocation
{
    [UInt(32)] public uint PhasesThatPerceiveMe { get; set; }
    [UInt(32)] public uint PhasesIPerceive { get; set; }
}
