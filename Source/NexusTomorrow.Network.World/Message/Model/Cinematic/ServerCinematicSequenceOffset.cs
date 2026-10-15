using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

// Used to adjust the scheduled time cinematic actions will execute based on their Delay + this SequenceOffset.
// SequenceOffset in this message is always relative to the instanteous current time
[Message(GameMessageOpcode.ServerCinematicStartTime)]
[WireMessage(GameMessageOpcode.ServerCinematicStartTime, Direction.ServerToClient)]
public sealed partial class ServerCinematicSequenceOffset : IWritable
{
    [UInt(32)] public uint SequenceOffset { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SequenceOffset);
    }
}
