using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

// May send this multiple times while the player is stunned.
[WireMessage(GameMessageOpcode.ClientCCStateStunUpdate, Direction.ClientToServer)]
public sealed partial class ClientCCStateStunUpdate
{
    [UInt(8)] public CCStateStunVictimGameplay InputPressed { get; private set; }
    [UInt(8)] public CCStateStunVictimGameplay InputHeld { get; private set; }
}
