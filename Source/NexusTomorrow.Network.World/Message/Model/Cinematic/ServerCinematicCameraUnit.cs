using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicCameraSubject)]
[WireMessage(GameMessageOpcode.ServerCinematicCameraSubject, Direction.ServerToClient)]
public sealed partial class ServerCinematicCameraUnit : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool Target { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(UnitId);
        writer.Write(Target);
    }
}
