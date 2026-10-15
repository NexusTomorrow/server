using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicTransitionPosition)]
public class ServerCinematicTransitionPosition : IWritable
{
    public uint Delay { get; set; }
    public Position Position { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        Position.Write(writer);
    }
}
