using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicVisualEffectAdd)]
[WireMessage(GameMessageOpcode.ServerCinematicVisualEffectAdd, Direction.ServerToClient)]
public sealed partial class ServerCinematicVisualEffectAdd : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint VisualEffectUniqueId { get; set; }
    [UInt(17)] public uint VisualEffectId { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    public Position Position { get; set; }
    [UInt(1)] public bool RemoveOnCameraEnd { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(VisualEffectUniqueId);
        writer.Write(VisualEffectId, 17u);
        writer.Write(UnitId);
        Position.Write(writer);
        writer.Write(RemoveOnCameraEnd);
    }
}
