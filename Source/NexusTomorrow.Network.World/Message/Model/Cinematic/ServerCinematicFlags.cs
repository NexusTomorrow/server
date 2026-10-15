using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicFlags)]
[WireMessage(GameMessageOpcode.ServerCinematicFlags, Direction.ServerToClient)]
public sealed partial class ServerCinematicFlags : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint Flags { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(Flags);
    }
}
