using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Client only processes message when on CharacterSelect screen.
[WireMessage(GameMessageOpcode.ServerQueueStatus, Direction.ServerToClient)]
public sealed partial class ServerQueueStatus
{
    [UInt(32)] public uint QueuePosition { get; set; }
    [UInt(32)] public uint WaitTimeSeconds { get; set; }
    [UInt(1)] public bool IsGuest { get; set; }
}
