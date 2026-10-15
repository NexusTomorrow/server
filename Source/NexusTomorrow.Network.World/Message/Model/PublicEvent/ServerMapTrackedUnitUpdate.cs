using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerMapTrackedUnitUpdate)]
[WireMessage(GameMessageOpcode.ServerMapTrackedUnitUpdate, Direction.ServerToClient)]
public sealed partial class ServerMapTrackedUnitUpdate : IWritable
{
    [UInt(32)] public uint TrackedUnitId { get; set; }
    [Float32] public Vector3 Position { get; set; }
    [UInt(14)] public uint PublicEventObjectiveId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TrackedUnitId);
        writer.Write(Position.X);
        writer.Write(Position.Y);
        writer.Write(Position.Z);
        writer.Write(PublicEventObjectiveId, 14u);
    }
}
