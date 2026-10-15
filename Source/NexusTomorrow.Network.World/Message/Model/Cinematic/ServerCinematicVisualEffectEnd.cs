using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicVisualEffectEnd)]
[WireMessage(GameMessageOpcode.ServerCinematicVisualEffectEnd, Direction.ServerToClient)]
public sealed partial class ServerCinematicVisualEffectEnd : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint VisualEffectUniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(VisualEffectUniqueId);
    }
}
