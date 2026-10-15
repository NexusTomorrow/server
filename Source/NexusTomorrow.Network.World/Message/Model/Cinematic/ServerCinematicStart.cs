using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicStart)]
[WireMessage(GameMessageOpcode.ServerCinematicStart, Direction.ServerToClient)]
public sealed partial class ServerCinematicStart : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(1)] public bool Show { get; set; }
    [UInt(1)] public bool Animate { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(Show);
        writer.Write(Animate);
    }
}
