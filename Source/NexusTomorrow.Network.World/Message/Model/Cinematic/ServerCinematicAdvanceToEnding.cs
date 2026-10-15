using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

// TODO:: confirm
[Message(GameMessageOpcode.ServerCinematicAdvanceToEnding)]
[WireMessage(GameMessageOpcode.ServerCinematicAdvanceToEnding, Direction.ServerToClient)]
public sealed partial class ServerCinematicAdvanceToEnding : IWritable
{
    [UInt(8)] public byte Unused { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unused);
    }
}
