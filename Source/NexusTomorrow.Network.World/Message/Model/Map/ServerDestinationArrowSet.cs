using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[Message(GameMessageOpcode.ServerDestinationArrowSet)]
[WireMessage(GameMessageOpcode.ServerDestinationArrowSet, Direction.ServerToClient)]
public sealed partial class ServerDestinationArrowSet : IWritable
{
    [Float32] public Vector3 DestinationPosition { get; set; }
    [Float32] public float Radius { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteVector3(DestinationPosition);
        writer.Write(Radius);
    }
}
