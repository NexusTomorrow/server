using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.ServerDestinationArrowSet, Direction.ServerToClient)]
public sealed partial class ServerDestinationArrowSet
{
    [Float32] public Vector3 DestinationPosition { get; set; }
    [Float32] public float Radius { get; set; }
}
