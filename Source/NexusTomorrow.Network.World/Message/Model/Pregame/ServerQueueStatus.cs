using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Client only processes message when on CharacterSelect screen.
[Message(GameMessageOpcode.ServerQueueStatus)]
[WireMessage(GameMessageOpcode.ServerQueueStatus, Direction.ServerToClient)]
public sealed partial class ServerQueueStatus : IWritable
{
    [UInt(32)] public uint QueuePosition { get; set; }
    [UInt(32)] public uint WaitTimeSeconds { get; set; }
    [UInt(1)] public bool IsGuest { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(QueuePosition);
        writer.Write(WaitTimeSeconds);
        writer.Write(IsGuest);
    }
}
