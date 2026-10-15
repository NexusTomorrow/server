using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicVisualEffectEnd)]
public class ServerCinematicVisualEffectEnd : IWritable
{
    public uint Delay { get; set; }
    public uint VisualEffectUniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(VisualEffectUniqueId);
    }
}
