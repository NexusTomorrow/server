using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicCameraAttach)]
[WireMessage(GameMessageOpcode.ServerCinematicCameraAttach, Direction.ServerToClient)]
public sealed partial class ServerCinematicCameraAttach : IWritable
{
    [UInt(32)] public uint AttachType { get; set; }
    [UInt(32)] public uint AttachId { get; set; }
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint ParentUnitId { get; set; }
    [UInt(1)] public bool UseRotation { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AttachType);
        writer.Write(AttachId);
        writer.Write(Delay);
        writer.Write(ParentUnitId);
        writer.Write(UseRotation);
    }
}
