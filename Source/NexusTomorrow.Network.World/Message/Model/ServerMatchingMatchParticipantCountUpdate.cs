using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Increments the CurrentAlly count and decrements the PendingAlly count if true
// Increments the CurrentEnemy count and decrements the PendingEnemy count if false
[WireMessage(GameMessageOpcode.ServerMatchingMatchParticipantCountUpdate, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchParticipantCountUpdate
{
    [UInt(1)] public bool Ally { get; set; } // true = ally, false = enemy
}
