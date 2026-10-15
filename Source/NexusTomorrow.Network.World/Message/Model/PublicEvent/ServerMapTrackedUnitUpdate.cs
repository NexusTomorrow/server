using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerMapTrackedUnitUpdate, Direction.ServerToClient)]
public sealed partial class ServerMapTrackedUnitUpdate
{
    [UInt(32)] public uint TrackedUnitId { get; set; }
    [Float32] public Vector3 Position { get; set; }
    [UInt(14)] public uint PublicEventObjectiveId { get; set; }
}
